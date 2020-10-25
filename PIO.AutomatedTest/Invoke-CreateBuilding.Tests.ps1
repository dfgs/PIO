
Describe 'Test CreateBuilding module'{
    .\BeforeAll.ps1

    Context 'Invoke-CreateBuilding' {
        
        It 'Given invalid WorkerID, it returns not task' {
            {Invoke-CreateBuilding 999 1 Sawmill}  | Should -Throw -ExceptionType ([System.ServiceModel.FaultException])
        }

        It 'Given invalid PlanetID, it returns not task' {
            {Invoke-CreateBuilding 1 999 Sawmill}  | Should -Throw -ExceptionType ([System.ServiceModel.FaultException])
        }
        
        It 'Given WorkerID, PlanetID and FactoryTypeID it creates building' {
           
            $buildingCount= (Get-Buildings 1).Count

            $task=Invoke-CreateBuilding 1 1 Sawmill

            $result = Get-Task $task.TaskID
            $result | Should -Not -BeNullOrEmpty
            Wait-ETA $task.ETA
            $result = Get-Task $task.TaskID
            $result | Should -BeNullOrEmpty

            $newBuildingCount= (Get-Buildings 1).Count
            $newBuildingCount | Should -Be ($buildingCount+1)

        }


    }
    

    .\AfterAll.ps1

}

