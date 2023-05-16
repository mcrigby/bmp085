using System.Device.I2c;

namespace CutilloRigby.Device.BMP085;

public sealed class Calibration
{
    public Int16 AC1 { get; private set; }
    public Int16 AC2 { get; private set; }
    public Int16 AC3 { get; private set; }
    public UInt16 AC4 { get; private set; }
    public UInt16 AC5 { get; private set; }
    public UInt16 AC6 { get; private set; }
    public Int16 B1 { get; private set; }
    public Int16 B2 { get; private set; }
    public Int16 MB { get; private set; }
    public Int16 MC { get; private set; }
    public Int16 MD { get; private set; }

    public static Calibration Load(I2cDevice i2cDevice)
    {
        const byte calibrationByteCount = 22;

        byte[] writeBuffer = new byte[] { (byte)Register.AC1_MSB };
        byte[] readBuffer = new byte[calibrationByteCount];
        i2cDevice.WriteRead(writeBuffer, readBuffer);

        var result = new Calibration();

        result.AC1 = (short) (( readBuffer[0] << 8) +  readBuffer[1]);
        result.AC2 = (short) (( readBuffer[2] << 8) +  readBuffer[3]);
        result.AC3 = (short) (( readBuffer[4] << 8) +  readBuffer[5]);
        result.AC4 = (ushort)(( readBuffer[6] << 8) +  readBuffer[7]);
        result.AC5 = (ushort)(( readBuffer[8] << 8) +  readBuffer[9]);
        result.AC6 = (ushort)((readBuffer[10] << 8) + readBuffer[11]);
        result.B1  = (short) ((readBuffer[12] << 8) + readBuffer[13]);
        result.B2  = (short) ((readBuffer[14] << 8) + readBuffer[15]);
        result.MB  = (short) ((readBuffer[16] << 8) + readBuffer[17]);
        result.MC  = (short) ((readBuffer[18] << 8) + readBuffer[19]);
        result.MD  = (short) ((readBuffer[20] << 8) + readBuffer[21]);

        return result;
    }

    public static Calibration Example()
    {
        var result = new Calibration();

        result.AC1 = 408;
        result.AC2 = -72;
        result.AC3 = -14383;
        result.AC4 = 32741;
        result.AC5 = 32757;
        result.AC6 = 23153;
        result.B1  = 6190;
        result.B2  = 4;
        result.MB  = -32768;
        result.MC  = -8711;
        result.MD  = 2868;

        return result;
    }
}
