BeforeAll { 
    Import-Module D:\Projets\PIO\PIO.PowerShell\bin\Debug\PIO.PowerShell.dll
    
    $global:process = Get-Process PIO.ServerHost -ErrorAction SilentlyContinue
    if ($global:process -eq $null) {
        $global:process = Start-Process D:\Projets\PIO\PIO.ServerHost\bin\Debug\PIO.ServerHost.exe -PassThru
        Start-Sleep -Seconds 2
    }
}