
Describe 'Test Building module'{
    .\BeforeAll.ps1

    Context 'Get-Buildings' {
        It 'Given no parameters, it lists all Building' {
            $result = Get-Buildings 1
            $result.Count | Should -Not -Be 0
            Foreach($element in $collection) 
            { 
                $element.PlanetID | Should -Be 1
            }
        }
    }
    Context 'Get-Building' {
        It 'Given BuildingID, it returns 1 Building' {
            $result = Get-Building 1
            $result | Should -Not -BeNullOrEmpty
        }
        It 'Given X and Y, it returns 1 Building' {
            $result = Get-Building -X 0 -Y 0
            $result | Should -Not -BeNullOrEmpty
        }
        It 'Given incorrect BuildingID, it returns 0 Building' {
            $result = Get-Building 999
            $result | Should -BeNullOrEmpty
        }
    }

    .\AfterAll.ps1

}

