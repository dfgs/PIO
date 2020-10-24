
Describe 'Test BuildingTypes module'{
    .\BeforeAll.ps1

    Context 'Get-BuildingTypes' {
        It 'Given no parameters, it lists all BuildingType' {
            $result = Get-BuildingTypes
            $result.Count | Should -Not -Be 0
        }
    }
    Context 'Get-BuildingType' {
        It 'Given BuildingTypeID, it returns 1 BuildingType' {
            $result = Get-BuildingType Factory
            $result | Should -Not -BeNullOrEmpty
            $result.BuildingTypeID | Should -Be Factory
            $result.Name | Should -Not -BeNullOrEmpty
            $result.HealthPoints | Should -Not -Be 0
        }
       
    }

    .\AfterAll.ps1

}

