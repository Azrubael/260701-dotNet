### 2026-07-16
-------------------

*JSON Azure Resource Manager templates* (ARM templates) allow you to specify your project's infrastructure in a declarative and reusable way.

`Bicep` is a language for declaratively defining your Azure resources. It has a simpler authoring experience than `JSON`, along with other features that help improve the quality of your infrastructure as code.
`Bicep` helps reduce manual deployment operations so you can scale your solutions more easily and with higher quality and consistency.
When you submit a `Bicep` file to Resource Manager, the Bicep tooling converts your file to a JSON format in a process called `transpilation`.

### If you neet to install Azure CLI
```pwsh
winget --version
winget search --name Azure CLI
winget install Microsoft.AzureCLI
az version
az login
```

### If you neet to install Golang
```
PS C:\Users\User> winget search GoLang.Go
Name                    Id        Version Source
-------------------------------------------------
Go Programming Language GoLang.Go 1.26.5  winget
PS C:\Users\User> winget install GoLang.Go
PS C:\Users\User> go version
go version go1.26.5 windows/amd64
```

`Infrastructure as code ` (IaC) allows you to describe, through code, the infrastructure that you need for your application.

With *IaC*, you can maintain both your application code and everything you need to deploy your application in a central code repository. The advantages to infrastructure as code are:
+ Consistent configurations
+ Improved scalability
+ Faster deployments
+ Better traceability

```pwsh
PS C:\Users\User> ."C:\Program Files\Microsoft SDKs\Azure\CLI2\wbin\az.cmd"
```

[1] Create an ARM template
```JSON
{
  "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
  "contentVersion": "1.0.0.0",
  "parameters": {},
  "functions": [],
  "variables": {},
  "resources": [],
  "outputs": {}
}
```

[2] Sign in to Azure by using Azure PowerShell
```pwsh
Connect-AzAccount
```

[3] Deploy the template to Azure
```pwsh
New-AzResourceGroup -Name <ResourceGroupName> -Location <Location>
Set-AzDefault -ResourceGroupName <ResourceGroupName>
```

[4] Deploy the template to Azure by running the following commands. The ARM template doesn't have any resources yet, so there aren't any resources created.
```pwsh
$templateFile="azuredeploy.json"
$today=Get-Date -Format "MM-dd-yyyy"
$deploymentName="blanktemplate-"+"$today"
New-AzResourceGroupDeployment -Name $deploymentName -TemplateFile $templateFile
```
The top section of the preceding code sets Azure PowerShell variables, which includes the path to the deployment file and the name of the deployment. Then, the New-AzResourceGroupDeployment command deploys the template to Azure. Notice that the deployment name is blanktemplate with the date as a suffix.

[5] When you deploy your ARM template to Azure, go to:
https://portal.azure.com
5.1 - In the resource menu, select Resource groups.
5.2 - Select the resource group you created in this exercise.
5.3 - On the Overview pane, you see that one deployment succeeded.
5.4 - Select 1 Succeeded to see the details of the deployment.
5.5 - Select blanktemplate to see what resources were deployed. In this case, it's empty because you didn't specify any resources in the template yet.
5.6 - Leave the page open in your browser so that you can check on deployments again.

[6] Add a resource to the ARM template
> type azuredeploy.JSON
```JSON
{
  "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
  "contentVersion": "1.0.0.0",
  "parameters": {},
  "functions": [],
  "variables": {},
  "resources": [
    {
      "type": "Microsoft.Storage/storageAccounts",
      "apiVersion": "2025-01-01",
      "name": "azrubael2025",
      "tags": {
        "displayName": "azrubael2025"
      },
      "location": "[resourceGroup().location]",
      "kind": "StorageV2",
      "sku": {
        "name": "Standard_LRS"
      }
    }
  ],
  "outputs": {}
}
```

[7] Deploy the updated ARM template
```pwsh
$templateFile="azuredeploy.json"
$today=Get-Date -Format "MM-dd-yyyy"
$deploymentName="addstorage-"+"$today"
New-AzResourceGroupDeployment -Name $deploymentName -TemplateFile $templateFile
```

[8] Check your deployment
When the deployment finishes, go back to the Azure portal in your browser. Go to your resource group, and you see that there are now 2 Succeeded deployments. Select this link.

Notice that both deployments are in the list.