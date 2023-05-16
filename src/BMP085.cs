using System.Device.I2c;

namespace CutilloRigby.Device.BMP085;

public sealed class BMP085 : IDisposable
{
    private readonly I2cDevice _i2cDevice;

    private Calibration _calibration;
    private Mode _mode;

    /// <summary>
    /// Creates a new instance of the BMP085
    /// </summary>
    /// <param name="i2cDevice">The I2C device used for communication.</param>
    public BMP085(I2cDevice i2cDevice)
    {
        _i2cDevice = i2cDevice ?? throw new ArgumentNullException(nameof(i2cDevice));

        if (!Init())
            throw new Exception("Unable to communicate with BMP085");

        _calibration = Calibration.Load(_i2cDevice);
        _mode = (byte)RequiredMeasurement.Temperature;

        GetRawMeasurement = () => 0;
        BuildGetRawMeasurement();
    }

    /// <summary>
    /// Default BMP085 I2C Address
    /// </summary>
    public const byte DefaultI2CAddress = 0x77;

    /// <summary>
    /// BMP085 RawMeasurement
    /// </summary>
    public long RawMeasurement => GetRawMeasurement();

    /// <summary>
    /// Checks if the device is a BMP085
    /// </summary>
    /// <returns>True if device has been correctly detected</returns>
    private bool Init()
    {
        return _i2cDevice.ReadByte((byte)Register.AC1_MSB) != 0x00;
    }

    public Calibration Calibration => _calibration;

    public RequiredMeasurement Measurement
    {
        get => _mode.Measurement;
        set
        {
            _mode.Measurement = value;
            BuildGetRawMeasurement();
        }
    }
    public OversamplingSetting Oversampling
    {
        get => _mode.Oversampling;
        set
        {
            if (_mode.Measurement == RequiredMeasurement.Temperature)
                return;
                
            _mode.Oversampling = value;
            BuildGetRawMeasurement();
        }
    }

    /// <summary>
    /// Read BMP085 Raw Measurement Value
    /// </summary>
    /// <returns>Raw Pressure or Temperature Value, depending on Mode</returns>
    public Func<Int32> GetRawMeasurement;

    private void BuildGetRawMeasurement()
    {
        var delay = GetConversionTimems(_mode);
        var adjust = (byte)(8 - ((byte)_mode.Oversampling >> 6));

        GetRawMeasurement = _mode.Measurement switch
        {
            RequiredMeasurement.Temperature => () => {                
                _i2cDevice.WriteByte((byte)Register.Mode, (byte)_mode, x => delay);

                byte[] writeBuffer = new byte[] { (byte)Register.Measurement_MSB };
                byte[] readBuffer = new byte[2];
                _i2cDevice.WriteRead(writeBuffer, readBuffer);
                    
                return (Int32)((Int32)readBuffer[0] << 8) + (Int32)readBuffer[1];
            },
            RequiredMeasurement.Pressure => () => {
                _i2cDevice.WriteByte((byte)Register.Mode, (byte)_mode, x => delay);

                byte[] writeBuffer = new byte[] { (byte)Register.Measurement_MSB };
                byte[] readBuffer = new byte[3];
                _i2cDevice.WriteRead(writeBuffer, readBuffer);
                    
                var result = (Int32)((Int32)readBuffer[0] << 16) + (Int32)((Int32)readBuffer[1] << 8) + (Int32)readBuffer[2];
                return (Int32)(result >> adjust);
            },
            _ => () => 0
        };
    }

    /// <summary>
    /// Cleanup
    /// </summary>
    public void Dispose()
    {
        _i2cDevice?.Dispose();
    }

    public static BMP085 Create(byte address = DefaultI2CAddress, Int32 busId = 1)
    {
        var settings = new I2cConnectionSettings(busId, address);
        var device = I2cDevice.Create(settings);

        return new BMP085(device);
    }

    private static byte GetConversionTimems(Mode mode)
    {
        return mode.Oversampling switch
        {
            OversamplingSetting.UltraLowPower => 5,
            OversamplingSetting.Standard => 8,
            OversamplingSetting.HighResolution => 14,
            OversamplingSetting.UltraHighResolution => 26,
            _ => 0
        };
    }
}
