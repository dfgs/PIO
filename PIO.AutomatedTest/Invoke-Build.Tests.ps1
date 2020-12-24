
Describe 'Test BuildFactory module'{
    .\BeforeAll.ps1

    Context 'Invoke-BuildFactory' {
        
        It 'Given invalid WorkerID, it returns not task' {
            {Invoke-BuildFactory 999 }  | Should -Throw -ExceptionType ([System.ServiceModel.FaultException])
        }
        It 'Given empty position, it returns not task' {
            (Invoke-MoveTo -WorkerID 1 -X -10 -Y -10) | Wait-Task
            {Invoke-BuildFactory 1 }  | Should -Throw -ExceptionType ([System.ServiceModel.FaultException])
        }

        
        It 'Given WorkerID and FactoryTypeID it creates building and build it to the end' {
            $forest = ((Get-Factories 1) | Where-Object FactoryTypeID -eq Forest)[0]

            (Invoke-MoveTo -WorkerID 1 -X 11 -Y 11) | Wait-Task
            $task=(Invoke-CreateBuilding 1 Sawmill) | Wait-Task
            $sawmill=Get-Factory -PlanetID 1 -X 11 -Y 11


            while($sawmill.RemainingBuildSteps -gt 0)
            {
                # prepare materials 
                (Invoke-MoveTo -WorkerID 1 -X $forest.X -Y $forest.Y) | Wait-Task
                $task=(Invoke-Produce 1) | Wait-Task
                $task=(Invoke-CarryTo 1 $sawmill.BuildingID Wood) | Wait-Task

                # build 
                $task=Invoke-BuildFactory 1
                $result = Get-Task $task.TaskID
                $result | Should -Not -BeNullOrEmpty
                $task | Wait-Task
                $result = Get-Task $task.TaskID
                $result | Should -BeNullOrEmpty
 
                $sawmill=Get-Factory -PlanetID 1 -X 11 -Y 11
            }
           
            # nothing more to build
            {Invoke-BuildFactory 1 }  | Should -Throw -ExceptionType ([System.ServiceModel.FaultException])

        }


    }
    

    .\AfterAll.ps1

}

