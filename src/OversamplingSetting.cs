namespace CutilloRigby.Device.BMP085;

/// <summary>
///
/// </summary>
public enum OversamplingSetting : byte
{
    /// <summary>
    /// Internal Number of Samples = 1,
    /// Conversion Time = 4.5ms,
    /// Avg. current @ 1 sample/s = 2uA
    /// RMS noise = 0.06hPa or 0.5m
    /// </summary>
    UltraLowPower = 0x00,
    /// <summary>
    /// Internal Number of Samples = 1,
    /// Conversion Time = 7.5ms,
    /// Avg. current @ 1 sample/s = 5uA
    /// RMS noise = 0.05hPa or 0.4m
    /// </summary>
    Standard = 0x40,
    /// <summary>
    /// Internal Number of Samples = 1,
    /// Conversion Time = 13.5ms,
    /// Avg. current @ 1 sample/s = 7uA
    /// RMS noise = 0.04hPa or 0.3m
    /// </summary>
    HighResolution = 0x80,
    /// <summary>
    /// Internal Number of Samples = 1,
    /// Conversion Time = 25.5ms,
    /// Avg. current @ 1 sample/s = 12uA
    /// RMS noise = 0.03hPa or 0.25m
    /// </summary>
    UltraHighResolution = 0xc0,
}