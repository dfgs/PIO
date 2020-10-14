
Describe 'Test Ingredient module'{
    .\BeforeAll.ps1

    Context 'Get-Ingredients' {
        It 'Given FactoryTypeID, it lists all Ingredient' {
            $result = Get-Ingredients Sawmill
            $result.Count | Should -Not -Be 0
            Foreach($element in $collection) 
            { 
                $element.FactoryTypeID | Should -Be Sawmill
                $element.Quantity | Should -Not -Be 0
            }
        }
    }
    Context 'Get-Ingredient' {
        It 'Given IngredientID, it returns 1 Ingredient' {
            $result = Get-Ingredient 1
            $result | Should -Not -BeNullOrEmpty
            $element.Quantity | Should -Not -Be 0
        }
        It 'Given incorrect IngredientID, it returns 0 Ingredient' {
            $result = Get-Ingredient 999
            $result | Should -BeNullOrEmpty
        }
    }

    .\AfterAll.ps1

}

