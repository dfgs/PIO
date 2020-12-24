
Describe 'Test CreateBuilding module'{
    .\BeforeAll.ps1

    Context 'Invoke-CreateBuilding' {
        
        It 'Given invalid WorkerID, it returns not task' {
            {Invoke-CreateBuilding 999 Sawmill}  | Should -Throw -ExceptionType ([System.ServiceModel.FaultException])
        }
        It 'Given occupied pôsition, it returns not task' {
            (Invoke-MoveTo -WorkerID 1 -X 0 -Y 0) | Wait-Task
            {Invoke-CreateBuilding 1 Sawmill}  | Should -Throw -ExceptionType ([System.ServiceModel.FaultException])
        }

        
        It 'Given WorkerID and FactoryTypeID it creates building' {
            (Invoke-MoveTo -WorkerID 1 -X 5 -Y 5) | Wait-Task
           
            $buildingCount= (Get-Buildings 1).Count

            $task=Invoke-CreateBuilding 1 Sawmill

            $result = Get-Task $task.TaskID
            $result | Should -Not -BeNullOrEmpty
            $task | Wait-Task
            $result = Get-Task $task.TaskID
            $result | Should -BeNullOrEmpty

            $newBuildingCount= (Get-Buildings 1).Count
            $newBuildingCount | Should -Be ($buildingCount+1)

        }


    }
    

    .\AfterAll.ps1

}

