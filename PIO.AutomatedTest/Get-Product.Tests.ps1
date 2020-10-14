
Describe 'Test Product module'{
    .\BeforeAll.ps1

    Context 'Get-Products' {
        It 'Given FactoryTypeID, it lists all Product' {
            $result = Get-Products Forest
            $result.Count | Should -Not -Be 0
            Foreach($element in $collection) 
            { 
                $element.FactoryTypeID | Should -Be Forest
                $element.Duration | Should -Not -Be 0
                $element.Quantity | Should -Not -Be 0
            }
        }
    }
    Context 'Get-Product' {
        It 'Given ProductID, it returns 1 Product' {
            $result = Get-Product 1
            $result | Should -Not -BeNullOrEmpty
            $element.Duration | Should -Not -Be 0
            $element.Quantity | Should -Not -Be 0
        }
        It 'Given incorrect ProductID, it returns 0 Product' {
            $result = Get-Product 999
            $result | Should -BeNullOrEmpty
        }
    }

    .\AfterAll.ps1

}

