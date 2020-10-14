
Describe 'Test ResourceTypes module'{
    .\BeforeAll.ps1

    Context 'Get-ResourceTypes' {
        It 'Given no parameters, it lists all ResourceType' {
            $result = Get-ResourceTypes
            $result.Count | Should -Not -Be 0
        }
    }
    Context 'Get-ResourceType' {
        It 'Given ResourceTypeID, it returns 1 ResourceType' {
            $result = Get-ResourceType Wood
            $result | Should -Not -BeNullOrEmpty
            $result.ResourceTypeID | Should -Be Wood
            $result.Name | Should -Not -BeNullOrEmpty
        }
       
    }

    .\AfterAll.ps1

}

