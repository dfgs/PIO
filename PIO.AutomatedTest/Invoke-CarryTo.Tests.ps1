
Describe 'Test CarryTo module'{
    .\BeforeAll.ps1

    Context 'Invoke-CarryTo' {
        
        It 'Given invalid WorkerID, it returns not task' {
            {Invoke-CarryTo 999 1 Tree}  | Should -Throw -ExceptionType ([System.ServiceModel.FaultException])
        }

        It 'Given invalid TargetFactoryID, it returns not task' {
            {Invoke-CarryTo 1 999 Tree}  | Should -Throw -ExceptionType ([System.ServiceModel.FaultException])
        }
        
        It 'Given WorkerID, TargetFactoryID and ResourceTypeID it carries to new location' {
            $worker=Get-Worker 1
            $source = ((Get-Factories 1) | Where-Object FactoryTypeID -eq Forest)[0]
            $target = ((Get-Factories 1) | Where-Object FactoryTypeID -eq StockPile)[0]
            
            $task=Invoke-MoveTo $worker.WorkerID $source.FactoryID
            Wait-ETA $task.ETA

            $sourceQuantity=Get-StackQuantity $source.FactoryID Tree
            $targetQuantity=Get-StackQuantity $target.FactoryID Tree
            $task=Invoke-CarryTo $worker.WorkerID $target.FactoryID Tree
            
            $result = Get-Task $task.TaskID
            $result | Should -Not -BeNullOrEmpty
            Wait-ETA $task.ETA
            $result = Get-Task $task.TaskID
            $result | Should -BeNullOrEmpty
      
            $result=Get-Worker 1
            $result.FactoryID | Should -Be $target.FactoryID
 
            $sourceNewQuantity=Get-StackQuantity $source.FactoryID Tree
            $targetNewQuantity=Get-StackQuantity $target.FactoryID Tree

            $sourceNewQuantity | Should -Be ($sourceQuantity-1)
            $targetNewQuantity | Should -Be ($targetQuantity+1)

        }


    }
    

    .\AfterAll.ps1

}

