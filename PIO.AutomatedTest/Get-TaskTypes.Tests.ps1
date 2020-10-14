
Describe 'Test TaskTypes module'{
    .\BeforeAll.ps1

    Context 'Get-TaskTypes' {
        It 'Given no parameters, it lists all TaskType' {
            $result = Get-TaskTypes
            $result.Count | Should -Not -Be 0
        }
    }
    Context 'Get-TaskType' {
        It 'Given TaskTypeID, it returns 1 TaskType' {
            $result = Get-TaskType CarryTo
            $result | Should -Not -BeNullOrEmpty
            $result.TaskTypeID | Should -Be CarryTo
            $result.Name | Should -Not -BeNullOrEmpty
        }
       
    }

    .\AfterAll.ps1

}

