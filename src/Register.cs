namespace CutilloRigby.Device.BMP085;

/// <summary>
/// The 176 bit E2PROM is partitioned in 11 words of 16 bit each. These contain 11 calibration
/// coefficients. Every sensor module has individual coefficients. Before the first calculation of
/// temperature and pressure, the master reads out the E2PROM data.
/// 
/// The data communication can be checked by checking that none of the words has the value 0 or
/// 0xFFFF.
/// </summary?
public enum Register
{
    AC1_MSB = 0xaa,
    AC1_LSB = 0xab,
    AC2_MSB = 0xac,
    AC2_LSB = 0xad,
    AC3_MSB = 0xae,
    AC3_LSB = 0xaf,
    AC4_MSB = 0xb0,
    AC4_LSB = 0xb1,
    AC5_MSB = 0xb2,
    AC5_LSB = 0xb3,
    AC6_MSB = 0xb4,
    AC6_LSB = 0xb5,
    B1_MSB = 0xb6,
    B1_LSB = 0xb7,
    B2_MSB = 0xb8,
    B2_LSB = 0xb9,
    MB_MSB = 0xba,
    MB_LSB = 0xbb,
    MC_MSB = 0xbc,
    MC_LSB = 0xbd,
    MD_MSB = 0xbe,
    MD_LSB = 0xbf,
    Mode = 0xf4,
    Measurement_MSB = 0xf6,
    Measurement_LSB = 0xf7,
    Measurement_XLSB = 0xf8,
}
