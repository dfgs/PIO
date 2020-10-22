Import-Module D:\Projets\PIO\PIO.PowerShell\bin\Debug\PIO.PowerShell.dll





$process = Start-Process D:\Projets\PIO\PIO.ServerHost\bin\Debug\PIO.ServerHost.exe -PassThru
Start-Sleep -Seconds 2

Invoke-Pester .\*.Tests.ps1

Stop-Process $process
#taskkill.exe /im PIO.ServerHost.exe

