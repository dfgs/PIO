
Describe 'Test MoveTo module'{
    .\BeforeAll.ps1

    Context 'Invoke-MoveTo' {
        
        It 'Given invalid Worker, it returns not task' {
            {Invoke-MoveTo 999 1}  | Should -Throw -ExceptionType ([System.ServiceModel.FaultException])
        }

        It 'Given invalid TargetFactory, it returns not task' {
            {Invoke-MoveTo 1 999}  | Should -Throw -ExceptionType ([System.ServiceModel.FaultException])
        }
        
        It 'Given Worker, it moves to new location' {
            $worker=Get-Worker 1
            $target = ((Get-Factories 1) | Where-Object FactoryID -ne $worker.FactoryID)[0]
            
            $task=Invoke-MoveTo $worker.WorkerID $target.FactoryID

            $result = Get-Task $task.TaskID
            $result | Should -Not -BeNullOrEmpty
            Wait-ETA $task.ETA
            $result = Get-Task $task.TaskID
            $result | Should -BeNullOrEmpty
      
            $result=Get-Worker 1
            $result.FactoryID | Should -Be $target.FactoryID
        }


    }
    

    .\AfterAll.ps1

}

