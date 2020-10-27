
Describe 'Test MoveTo module'{
    .\BeforeAll.ps1

    Context 'Invoke-MoveTo' {
        
        It 'Given invalid WorkerID, it returns not task' {
            {Invoke-MoveTo 999 1 1}  | Should -Throw -ExceptionType ([System.ServiceModel.FaultException])
        }

        
        
        It 'Given WorkerID, it moves to new location' {
            
            $task=Invoke-MoveTo 1 10 11

            $result = Get-Task $task.TaskID
            $result | Should -Not -BeNullOrEmpty
            $task | Wait-Task
            $result = Get-Task $task.TaskID
            $result | Should -BeNullOrEmpty
      
            $result=Get-Worker 1
            $result.X | Should -Be 10
            $result.Y | Should -Be 11
        }


    }
    

    .\AfterAll.ps1

}

