
Describe 'Test Task module'{
    .\BeforeAll.ps1

    Context 'Get-Tasks' {
        
        It 'Given Worker, it lists all Task' {
            $result = Get-Tasks 1
            $result.Count | Should -Be 0
            Invoke-Idle 1 5
            $result = Get-Tasks 1
            $result.Count | Should -Not -Be 0
            Foreach($element in $collection) 
            { 
                $element.WorkerID | Should -Be 1
                $element.TaskTypeID | Should -Be Idle
            }
            Start-Sleep -Seconds 5
            $result = Get-Tasks 1
            $result.Count | Should -Be 0
       }
    }
    Context 'Get-Task' {
        It 'Given TaskID, it returns 1 Task' {
            $task = Invoke-Idle 1 5
            $result = Get-Task $task.TaskID
            $result | Should -Not -BeNullOrEmpty
            $result.WorkerID | Should -Be 1
            $result.TaskTypeID | Should -Be Idle
        }
        It 'Given incorrect TaskID, it returns 0 Task' {
            $result = Get-Task 999
            $result | Should -BeNullOrEmpty
        }
    }

    .\AfterAll.ps1

}

