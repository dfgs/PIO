Import-Module D:\Projets\PIO\PIO.PowerShell\bin\Debug\PIO.PowerShell.dll


function Wait-ETA([DateTime] $ETA)
{
    $start= Get-Date
    $duration=NEW-TIMESPAN –Start $start –End $ETA
    Write-Host -ForegroundColor Yellow "Waiting for $duration seconds..."
    Start-Sleep -Seconds $duration.TotalSeconds
}


$process = Start-Process D:\Projets\PIO\PIO.ServerHost\bin\Debug\PIO.ServerHost.exe -PassThru
Start-Sleep -Seconds 2

Invoke-Pester .\*.Tests.ps1

Stop-Process $process
#taskkill.exe /im PIO.ServerHost.exe

