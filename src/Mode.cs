namespace CutilloRigby.Device.BMP085;

public sealed class Mode 
{
    private byte _value;

    public Mode(byte Value)
    {
        _value = Value;
    }

    public RequiredMeasurement Measurement
    {
        get => (RequiredMeasurement)(_value & 0x3f);
        set
        {
            _value &= 0x00; // 0x00 to reset Oversampling. To preserve Oversampling, use 0xc0
            _value |= (byte)value;
        }
    }
    public OversamplingSetting Oversampling
    {
        get => (OversamplingSetting)(_value & 0xc0);
        set
        {
            _value &= 0x3f;
            _value |= (byte)value;
        }
    }

    /// <summary>
    /// When Measurement is Temperature, Oversampling must be UltraLowPower.
    /// </summary>
    public bool IsValid => Measurement == RequiredMeasurement.Pressure ||
        (Measurement == RequiredMeasurement.Temperature && Oversampling == OversamplingSetting.UltraLowPower);

    public static implicit operator byte(Mode mode) => mode._value;
    public static implicit operator Mode(byte value) => new Mode(value);
}