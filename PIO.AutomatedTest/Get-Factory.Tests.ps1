
Describe 'Test Factory module'{
    .\BeforeAll.ps1

    Context 'Get-Factories' {
        It 'Given no parameters, it lists all Factory' {
            $result = Get-Factories 1
            $result.Count | Should -Not -Be 0
            Foreach($element in $collection) 
            { 
                $element.PlanetID | Should -Be 1
            }
        }
    }
    Context 'Get-Factory' {
        It 'Given FactoryID, it returns 1 Factory' {
            $result = Get-Factory 1
            $result | Should -Not -BeNullOrEmpty
        }
        It 'Given incorrect FactoryID, it returns 0 Factory' {
            $result = Get-Factory 999
            $result | Should -BeNullOrEmpty
        }
    }

    .\AfterAll.ps1

}

