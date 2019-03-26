    param(
     [Parameter(Mandatory=$True, position=0)]
     [String] $resourceGroupName, 
     [ValidateNotNullOrEmpty()]
     [Parameter(position=1)]
     [String[]]$ResourceTypes = $("Microsoft.Sql/servers","Microsoft.Web/sites")
     )
 

    foreach ($type in $ResourceTypes) 
    {
        Get-AzResource -ResourceGroupName $resourceGroupName -ResourceType $type | ForEach-Object{
            
            Write-Output "$($_.Name) found, trying to delete"
            $result = Remove-AzResource -ResourceGroupName $resourceGroupName -ResourceType $type -ResourceName $_.Name  -Force
            Write-Output "And this try end with: $($result)"
        }
    }





