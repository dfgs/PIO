
Describe 'Test FactoryTypes module'{
    .\BeforeAll.ps1

    Context 'Get-FactoryTypes' {
        It 'Given no parameters, it lists all FactoryType' {
            $result = Get-FactoryTypes
            $result.Count | Should -Not -Be 0
        }
    }
    Context 'Get-FactoryType' {
        It 'Given FactoryTypeID, it returns 1 FactoryType' {
            $result = Get-FactoryType Sawmill
            $result | Should -Not -BeNullOrEmpty
            $result.FactoryTypeID | Should -Be Sawmill
            $result.Name | Should -Not -BeNullOrEmpty
            $result.HealthPoints | Should -Not -Be 0
        }
       
    }

    .\AfterAll.ps1

}

