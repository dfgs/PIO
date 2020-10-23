BeforeAll { 
    Import-Module D:\Projets\PIO\PIO.PowerShell\bin\Debug\PIO.PowerShell.dll
    
    function Wait-ETA([DateTime] $end) {
        $start = Get-Date
        $duration = New-TimeSpan -Start $start -End $end
        Write-Host -ForegroundColor Yellow "Waiting for $duration seconds..."
        Start-Sleep -Seconds ($duration.TotalSeconds+1);
    }
    
    $global:process = Get-Process PIO.ServerHost -ErrorAction SilentlyContinue
    if ($global:process -eq $null) {
        $global:process = Start-Process D:\Projets\PIO\PIO.ServerHost\bin\Debug\PIO.ServerHost.exe -PassThru
        Start-Sleep -Seconds 2
    }
}