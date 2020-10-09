Import-Module D:\Projets\PIO\PIO.PowerShell\bin\Debug\PIO.PowerShell.dll


$global:failedCount=0
$global:passedCount=0

function Write-Passed()
{
    $global:passedCount++
    Write-Host -ForegroundColor Green "Passed"
}
function Write-Failed()
{
    $global:failedCount++
    Write-Host -ForegroundColor Red "Failed"
}

function RunTest ([String] $Description,[Bool] $IsTrue, $IsNull, $IsNotNull)
{
    Write-Host -ForegroundColor Gray "$Description"

    switch($PSBoundParameters.Keys)
    {
        'IsTrue'
        {
            if ($IsTrue -eq $true) {Write-Passed -Result $Result}
            else {Write-Failed -Result $Result}
        }
        'IsNull'
        {
            if ($IsNull -eq $null) {Write-Passed -Result $Result}
            else {Write-Failed -Result $Result}
        }
        'IsNotNull'
        {
            if ($IsNotNull -ne $null) {Write-Passed -Result $Result}
            else {Write-Failed -Result $Result}
        }
    }
    
  
}



cls

# FactoryType
Write-Host
Write-Host -ForegroundColor White "Testing FactoryTypeModule:"
$result=Get-FactoryTypes
RunTest -Description "Get-FactoryTypes is not null" -IsNotNull $result
RunTest -Description "Get-FactoryTypes should return three results" -IsTrue ($result.Count -eq 3)

$result=Get-FactoryType Forest
RunTest -Description "Get-FactoryType is not null" -IsNotNull $result
RunTest -Description "FactoryTypeID is Forest" -IsTrue ($result.FactoryTypeID -eq 'Forest')
RunTest -Description "Name is Forest" -IsTrue ($result.Name -eq 'Forest')
RunTest -Description "HealthPoints is 999" -IsTrue ($result.HealthPoints -eq 999)
$result | Format-Table


# RessourceType
Write-Host
Write-Host -ForegroundColor White "Testing ResourceTypeModule:"
$result=Get-ResourceTypes
RunTest -Description "Get-ResourceTypes is not null" -IsNotNull $result
RunTest -Description "Get-ResourceTypes should return five results" -IsTrue ($result.Count -eq 5)

$result=Get-ResourceType Wood
RunTest -Description "Get-ResourceType is not null" -IsNotNull $result
RunTest -Description "ResourceTypeID is Wood" -IsTrue ($result.ResourceTypeID -eq 'Wood')
RunTest -Description "Name is Wood" -IsTrue ($result.Name -eq 'Wood')
$result | Format-Table

# TaskType
Write-Host
Write-Host -ForegroundColor White "Testing TaskTypeModule:"
$result=Get-TaskTypes
RunTest -Description "Get-TaskTypes is not null" -IsNotNull $result
RunTest -Description "Get-TaskTypes should return three results" -IsTrue ($result.Count -eq 3)

$result=Get-TaskType MoveTo
RunTest -Description "Get-TaskType is not null" -IsNotNull $result
RunTest -Description "TaskTypeID is MoveTo" -IsTrue ($result.TaskTypeID -eq 'MoveTo')
RunTest -Description "Name is MoveTo" -IsTrue ($result.Name -eq 'MoveTo')
$result | Format-Table



# Planet
Write-Host
Write-Host -ForegroundColor White "Testing PlanetModule:"
$result=Get-Planets
RunTest -Description "Get-Planets is not null" -IsNotNull $result
RunTest -Description "Get-Planets should return one result" -IsTrue ($result.Count -eq 1)

$result=Get-Planet 1
RunTest -Description "Get-Planet is not null" -IsNotNull $result
RunTest -Description "PlanetID is 1" -IsTrue ($result.PlanetID -eq 1)
RunTest -Description "Name is Default" -IsTrue ($result.Name -eq 'Default')
$result | Format-Table


$result=Get-Planet 0
RunTest -Description "Get-Planet is null" -IsNull $result



# Factory
Write-Host
Write-Host -ForegroundColor White "Testing FactoryModule:"
$result=Get-Factories 1
RunTest -Description "Get-Factories is not null" -IsNotNull $result
RunTest -Description "Get-Factories should return three results" -IsTrue ($result.Count -eq 3)

$result=Get-Factory 1
RunTest -Description "Get-Factory is not null" -IsNotNull $result
RunTest -Description "FactoryID is 1" -IsTrue ($result.FactoryID -eq 1)
RunTest -Description "PlanetID is 1" -IsTrue ($result.PlanetID -eq 1)
RunTest -Description "FactoryTypeID is Forest" -IsTrue ($result.FactoryTypeID -eq 'Forest')
$result | Format-Table

$result=Get-Factory 0
RunTest -Description "Get-Factory is null" -IsNull $result



# Worker
Write-Host
Write-Host -ForegroundColor White "Testing WorkerModule:"
$result=Get-Workers 1
RunTest -Description "Get-Workers is not null" -IsNotNull $result
RunTest -Description "Get-Workers should return one result" -IsTrue ($result.Count -eq 1)

$result=Get-Worker 1
RunTest -Description "Get-Worker is not null" -IsNotNull $result
RunTest -Description "WorkerID is 1" -IsTrue ($result.WorkerID -eq 1)
RunTest -Description "FactoryID is 1" -IsTrue ($result.FactoryID -eq 1)
$result | Format-Table

$result=Get-Worker 0
RunTest -Description "Get-Worker is null" -IsNull $result




# Ingredient
Write-Host
Write-Host -ForegroundColor White "Testing IngredientModule:"
$result=Get-Ingredients Forest
RunTest -Description "Get-Ingredients is not null" -IsNotNull $result
RunTest -Description "Get-Ingredients should return one results" -IsTrue ($result.Count -eq 1)

$result=Get-Ingredient 1
RunTest -Description "Get-Ingredient is not null" -IsNotNull $result
RunTest -Description "IngredientID is 1" -IsTrue ($result.IngredientID -eq 1)
RunTest -Description "FactoryTypeID is Forest" -IsTrue ($result.FactoryTypeID -eq 'Forest')
RunTest -Description "ResourceTypeID is Tree" -IsTrue ($result.ResourceTypeID -eq 'Tree')
RunTest -Description "Quantity is one" -IsTrue ($result.Quantity -eq 1)
$result | Format-Table

$result=Get-Ingredient 0
RunTest -Description "Get-Ingredient is null" -IsNull $result




# Material
Write-Host
Write-Host -ForegroundColor White "Testing MaterialModule:"
$result=Get-Materials Stockpile
RunTest -Description "Get-Materials is not null" -IsNotNull $result
RunTest -Description "Get-Materials should return one results" -IsTrue ($result.Count -eq 1)

$result=Get-Material 1
RunTest -Description "Get-Material is not null" -IsNotNull $result
RunTest -Description "MaterialID is 1" -IsTrue ($result.MaterialID -eq 1)
RunTest -Description "FactoryTypeID is Forest" -IsTrue ($result.FactoryTypeID -eq 'Forest')
RunTest -Description "ResourceTypeID is Wood" -IsTrue ($result.ResourceTypeID -eq 'Wood')
RunTest -Description "Quantity is one" -IsTrue ($result.Quantity -eq 1)
$result | Format-Table

$result=Get-Material 0
RunTest -Description "Get-Material is null" -IsNull $result



# Product
Write-Host
Write-Host -ForegroundColor White "Testing ProductModule:"
$result=Get-Products Forest
RunTest -Description "Get-Products is not null" -IsNotNull $result
RunTest -Description "Get-Products should return one results" -IsTrue ($result.Count -eq 1)

$result=Get-Product 1
RunTest -Description "Get-Product is not null" -IsNotNull $result
RunTest -Description "ProductID is 1" -IsTrue ($result.ProductID -eq 1)
RunTest -Description "FactoryTypeID is Forest" -IsTrue ($result.FactoryTypeID -eq 'Forest')
RunTest -Description "ResourceTypeID is Wood" -IsTrue ($result.ResourceTypeID -eq 'Wood')
RunTest -Description "Quantity is two" -IsTrue ($result.Quantity -eq 2)
RunTest -Description "Duration is one minute" -IsTrue ($result.Duration -eq 60)
$result | Format-Table

$result=Get-Product 0
RunTest -Description "Get-Product is null" -IsNull $result





# Stack
Write-Host
Write-Host -ForegroundColor White "Testing StackModule:"
$result=Get-Stacks 1
RunTest -Description "Get-Stacks is not null" -IsNotNull $result
RunTest -Description "Get-Stacks should return one result" -IsTrue ($result.Count -eq 1)

$result=Get-Stack 1
RunTest -Description "Get-Stack is not null" -IsNotNull $result
RunTest -Description "StackID is 1" -IsTrue ($result.StackID -eq 1)
RunTest -Description "FactoryID is 1" -IsTrue ($result.FactoryID -eq 1)
RunTest -Description "ResourceTypeID is Tree" -IsTrue ($result.ResourceTypeID -eq 'Tree')
RunTest -Description "Quantity is 100" -IsTrue ($result.Quantity -eq 100)
$result | Format-Table

$result=Get-Stack 0
RunTest -Description "Get-Stack is null" -IsNull $result














# todo GetLastTask

Write-Host 
Write-Host -ForegroundColor White "Test results:"
Write-Host -ForegroundColor Gray "Total passed: $global:passedCount"
Write-Host -ForegroundColor Gray "Total failed: $global:failedCount"
