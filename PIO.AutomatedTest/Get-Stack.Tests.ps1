
Describe 'Test Stack module'{
    .\BeforeAll.ps1

    Context 'Get-Stacks' {
        $script:factory = ((Get-Factories 1) | Where-Object FactoryTypeID -eq Forest)[0]
        
        It 'Given FactoryID, it lists all Stack' {
            $result = Get-Stacks $script:factory.FactoryID
            $result.Count | Should -Not -Be 0
            Foreach($element in $collection) 
            { 
                $element.FactoryID | Should -Be $script:factory.FactoryID
                $element.Quantity | Should -Not -Be 0
            }
        }
    }
    Context 'Get-Stack' {
        It 'Given StackID, it returns 1 Stack' {
            $result = Get-Stack 1
            $result | Should -Not -BeNullOrEmpty
            $result.FactoryID | Should -Not -Be 0
            $result.Quantity | Should -Not -Be 0
        }
        It 'Given incorrect StackID, it returns 0 Stack' {
            $result = Get-Stack 999
            $result | Should -BeNullOrEmpty
        }
    }
    Context 'Get-StackQuantity' {
        It 'Given StackID and ResourceTypeID, it returns a quantity' {
            $result = Get-StackQuantity 1 Tree
            $result.Quantity | Should -Not -Be 0
        }
        It 'Given incorrect StackID, it returns 0 ' {
            $result = Get-StackQuantity 999 Tree
            $result | Should -Be 0
        }
        It 'Given incorrect ResourceType, it returns 0 ' {
            $result = Get-StackQuantity 1 Coal
            $result | Should -Be 0
        }
    }

    .\AfterAll.ps1

}

