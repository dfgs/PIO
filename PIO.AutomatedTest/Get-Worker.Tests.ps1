
Describe 'Test Worker module'{
    .\BeforeAll.ps1

    Context 'Get-Workers' {
        It 'Given FactoryTypeID, it lists all Worker' {
            $result = Get-Workers 1
            $result.Count | Should -Not -Be 0
            Foreach($element in $collection) 
            { 
                $element.FactoryID | Should -Be 1
            }
        }
    }
    Context 'Get-Worker' {
        It 'Given WorkerID, it returns 1 Worker' {
            $result = Get-Worker 1
            $result | Should -Not -BeNullOrEmpty
            $result.FactoryID | Should -Not -Be 0
        }
        It 'Given incorrect WorkerID, it returns 0 Worker' {
            $result = Get-Worker 999
            $result | Should -BeNullOrEmpty
        }
    }

    .\AfterAll.ps1

}

