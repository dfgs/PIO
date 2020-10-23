
Describe 'Test Produce module'{
    .\BeforeAll.ps1

    Context 'Invoke-Produce' {
        
        It 'Given invalid WorkerID, it returns not task' {
            {Invoke-Produce 999 }  | Should -Throw -ExceptionType ([System.ServiceModel.FaultException])
        }

        It 'Given WorkerID, it produces wood' {
            $worker=Get-Worker 1
            $target = ((Get-Factories 1) | Where-Object FactoryTypeID -eq Forest)[0]
            
            $task=Invoke-MoveTo $worker.WorkerID $target.FactoryID
            Wait-ETA $task.ETA

            $initialTreeStackQuantity=Get-StackQuantity $target.FactoryID Tree
            $initialWoodStackQuantity=Get-StackQuantity $target.FactoryID Wood

            $task=Invoke-Produce $worker.WorkerID

            $result = Get-Task $task.TaskID
            $result | Should -Not -BeNullOrEmpty
            Wait-ETA $task.ETA
            $result = Get-Task $task.TaskID
            $result | Should -BeNullOrEmpty
      
            $newTreeStackQuantity=Get-StackQuantity $target.FactoryID Tree
            $newWoodStackQuantity=Get-StackQuantity $target.FactoryID Wood
          
            $newTreeStackQuantity | Should -BeLessThan $initialTreeStackQuantity
            $newWoodStackQuantity | Should -BeGreaterThan $initialWoodStackQuantity
        }


    }
    

    .\AfterAll.ps1

}

