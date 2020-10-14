
Describe 'Test Planet module'{
    .\BeforeAll.ps1

    Context 'Get-Planets' {
        It 'Given no parameters, it lists all Planet' {
            $result = Get-Planets
            $result.Count | Should -Not -Be 0
        }
    }
    Context 'Get-Planet' {
        It 'Given PlanetID, it returns 1 Planet' {
            $result = Get-Planet 1
            $result | Should -Not -BeNullOrEmpty
            $result.PlanetID | Should -Be 1
            $result.Name | Should -Not -BeNullOrEmpty
        }
        It 'Given incorrect PlanetID, it returns 0 Planet' {
            $result = Get-Planet 999
            $result | Should -BeNullOrEmpty
        }
    }

    .\AfterAll.ps1

}

