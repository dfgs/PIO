
Describe 'Test Material module'{
    .\BeforeAll.ps1

    Context 'Get-Materials' {
        It 'Given FactoryTypeID, it lists all Material' {
            $result = Get-Materials Stockpile
            $result.Count | Should -Not -Be 0
            Foreach($element in $collection) 
            { 
                $element.FactoryTypeID | Should -Be Stockpile
                $element.Quantity | Should -Not -Be 0
            }
        }
    }
    Context 'Get-Material' {
        It 'Given MaterialID, it returns 1 Material' {
            $result = Get-Material 1
            $result | Should -Not -BeNullOrEmpty
            $result.Quantity | Should -Not -Be 0
        }
        It 'Given incorrect MaterialID, it returns 0 Material' {
            $result = Get-Material 999
            $result | Should -BeNullOrEmpty
        }
    }

    .\AfterAll.ps1

}

