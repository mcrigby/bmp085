using UnitsNet;
using UnitsNet.Units;

namespace CutilloRigby.Device.BMP085;

public static class BMP085Extensions
{
    public static Int32 GetUT(this BMP085 bmp085)
    {
        bmp085.Measurement = RequiredMeasurement.Temperature;
        return bmp085.GetRawMeasurement();
    }
    public static Temperature GetTemperature(this BMP085 bmp085)
    {
        Int32 ut = bmp085.GetUT();
        if (ut == 0)
            return new Temperature(double.NaN, TemperatureUnit.DegreeCelsius);
        return bmp085.Calibration.GetTemperature(ut);
    }
    public static Temperature GetExampleTemperature(this BMP085 bmp085)
    {
        Int32 ut = 27898;
        return Calibration.Example().GetTemperature(ut);
    }
    private static Temperature GetTemperature(this Calibration calibration, Int32 ut)
    {
        Int32 x1 = 0, x2 = 0;

        var b5 = calibration.B5(ref ut, ref x1, ref x2);
        var t = (b5 + 8) >> 4;
        
        return new Temperature(t / 10.0, TemperatureUnit.DegreeCelsius);
    }

    public static Int32 GetUP(this BMP085 bmp085, OversamplingSetting oversampling)
    {
        bmp085.Measurement = RequiredMeasurement.Pressure;
        bmp085.Oversampling = oversampling;
        return bmp085.GetRawMeasurement();
    }
    public static Pressure GetPressure(this BMP085 bmp085, OversamplingSetting oversampling)
    {
        var ut = bmp085.GetUT();
        if (ut == 0)
            return new Pressure(double.NaN, PressureUnit.Pascal);

        Int32 up = bmp085.GetUP(oversampling);
        if (up == 0)
            return new Pressure(double.NaN, PressureUnit.Pascal);

        return bmp085.Calibration.GetPressure(oversampling, ut, up);
    }
    public static Pressure GetExamplePressure(this BMP085 bmp085, OversamplingSetting oversampling)
    {
        var oss = (byte)((byte)oversampling >> 6);

        Int32 ut = 27898;
        Int32 up = 23843 << oss;

        return Calibration.Example().GetPressure(oversampling, ut, up);
    }

    private static Pressure GetPressure(this Calibration calibration, OversamplingSetting oversampling, Int32 ut, Int32 up)
    {
        var oss = (byte)((byte)oversampling >> 6);
        Int32 x1 = 0, x2 = 0, x3 = 0;

        var b5 = calibration.B5(ref ut, ref x1, ref x2);
        var b6 = b5 - 4000;
        var b3 = calibration.B3(ref b6, ref oss, ref x1, ref x2, ref x3);
        var b4 = calibration.B4(ref b6, ref x1, ref x2, ref x3);
        var b7 = (UInt32)(((UInt32)up - b3) * (50000 >> oss));

        var p = P(ref b4, ref b7, ref x1, ref x2);

        return new Pressure(p, PressureUnit.Pascal);
    }

    private static Int32 B3(this Calibration calibration, ref Int32 b6, 
        ref byte oss, ref Int32 x1, ref Int32 x2, ref Int32 x3)
    {
        x1 = (calibration.B2 * (Int32)(b6 * b6 >> 12)) >> 11;
        x2 = (calibration.AC2 * b6) >> 11;
        x3 = x1 + x2;

        return (((((Int32)calibration.AC1 << 2) + x3) << oss) + 2) >> 2;
    }
    private static UInt32 B4(this Calibration calibration, ref Int32 b6, 
        ref Int32 x1, ref Int32 x2, ref Int32 x3)
    {
        x1 = (calibration.AC3 * b6) >> 13;
        x2 = (calibration.B1 * (Int32)(b6 * b6 >> 12)) >> 16;
        x3 = ((x1 + x2) + 2) >> 2;

        return (calibration.AC4 * (UInt32)((UInt32)x3 + 32768)) >> 15;
    }
    private static Int32 B5(this Calibration calibration, ref Int32 ut, ref Int32 x1, ref Int32 x2)
    {
        x1 = (ut - (Int32)calibration.AC6) * (Int32)calibration.AC5 >> 15;
        x2 = ((Int32)calibration.MC << 11) / (x1 + (Int32)calibration.MD);

        return x1 + x2;
    }
    private static Int32 P(ref UInt32 b4, ref UInt32 b7, ref Int32 x1, ref Int32 x2)
    {
        Int32 p;
        if (b7 < 0x80000000)
            p = (Int32)((b7 << 1) / b4);
        else
            p = (Int32)((b7 / b4) << 1);

        x1 = (p >> 8) * (p >> 8);
        x1 = (x1 * 3038) >> 16;
        x2 = (-7357 * p) >> 16;

        return p + ((x1 + x2 + 3791) >> 4);
    }
}
