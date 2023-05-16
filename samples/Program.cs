using CutilloRigby.Device.BMP085;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) => cts.Cancel();

var _bmp085 = BMP085.Create();

while(!cts.IsCancellationRequested)
{
    Console.WriteLine($"Temperature: {_bmp085.GetTemperature():n2}");
    
    //Console.WriteLine($"Example Pressure (Ultra Low Power): {_bmp085.GetExamplePressure(OversamplingSetting.UltraLowPower):n2}");
    //Console.WriteLine($"Example Pressure (Standard)       : {_bmp085.GetExamplePressure(OversamplingSetting.Standard):n2}");
    //Console.WriteLine($"Example Pressure (High Resolution): {_bmp085.GetExamplePressure(OversamplingSetting.HighResolution):n2}");
    //Console.WriteLine($"Example Pressure (Ultra High Res.): {_bmp085.GetExamplePressure(OversamplingSetting.UltraHighResolution):n2}");
    
    Console.WriteLine($"Actual  Pressure (Ultra Low Power): {_bmp085.GetPressure(OversamplingSetting.UltraLowPower).Hectopascals:n2}");
    Console.WriteLine($"Actual  Pressure (Standard)       : {_bmp085.GetPressure(OversamplingSetting.Standard).Hectopascals:n2}");
    Console.WriteLine($"Actual  Pressure (High Resolution): {_bmp085.GetPressure(OversamplingSetting.HighResolution).Hectopascals:n2}");
    Console.WriteLine($"Actual  Pressure (Ultra High Res.): {_bmp085.GetPressure(OversamplingSetting.UltraHighResolution).Hectopascals:n2}");
    
    Console.WriteLine();
    await Task.Delay(500);
}
