### 2026-07-16
-------------------

**QUIZ**
1. What is an Azure Resource Manager template?
A JavaScript Object Notation (JSON) file that defines the infrastructure and configuration for your deployment

2. Which one of these choices is `not` an element of an Azure Resource Manager template?
idempotent

3. Azure Resource Manager templates are idempotent. This means that if you run a template with no changes a second time
	Azure Resource Manager doesn't make any changes to the deployed resources



[1] n the azuredeploy.json file in Visual Studio Code, update `"parameters":{},`, so it looks like:
```json
"parameters": {
  "storageName": {
    "type": "string",
    "minLength": 3,
    "maxLength": 24,
    "metadata": {
      "description": "The name of the Azure storage resource"
    }
  }
},
```

[2] Use the new parameter in the `resources` block in both the `name` and `displayName` values. The entire file looks like this code example:
```json
{
  "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
  "contentVersion": "1.0.0.0",
  "parameters": {
    "storageName": {
      "type": "string",
      "minLength": 3,
      "maxLength": 24,
      "metadata": {
        "description": "The name of the Azure storage resource"
      }
    }
  },
  "functions": [],
  "variables": {},
  "resources": [
    {
      "type": "Microsoft.Storage/storageAccounts",
      "apiVersion": "2025-01-01",
      "name": "[parameters('storageName')]",
      "tags": {
        "displayName": "[parameters('storageName')]"
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

[3] Deploy the parameterized ARM template
```pwsh
templateFile="azuredeploy.json"
today=$(date +"%d-%b-%Y")
DeploymentName="addnameparameter-"$today

az deployment group create --name $DeploymentName --template-file $templateFile --parameters storageName={your-unique-name}
```

[4] Check your deployment
4.1 - When the deployment finishes, go back to the Azure portal in your browser. Go to your resource group, and see that there are now 3 Succeeded deployments. Select this link.
4.2 - Notice that all three deployments are in the list.
4.3 - Explore the addnameparameter deployment as you did previously.

[5] Add another parameter that limits allowed values
Add a new parameter named `storageSKU` to the `parameters` section of the `azuredeploy.json` file.
```json
// This is the allowed values for an Azure storage account
"storageSKU": {
   "type": "string",
   "defaultValue": "Standard_LRS",
   "allowedValues": [
     "Standard_LRS",
     "Standard_GRS",
     "Standard_RAGRS",
     "Standard_ZRS",
     "Premium_LRS",
     "Premium_ZRS",
     "Standard_GZRS",
     "Standard_RAGZRS"
   ]
 }
 ```
 
[6] Update resources to use the storageSKU parameter. If you take advantage of IntelliSense in Visual Studio Code, it makes this step easier.
```json
"sku": {
     "name": "[parameters('storageSKU')]"
   }
```

[7] The entire file looks like this code example:
```json
{
  "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
  "contentVersion": "1.0.0.0",
  "parameters": {
    "storageName": {
      "type": "string",
      "minLength": 3,
      "maxLength": 24,
      "metadata": {
        "description": "The name of the Azure storage resource"
      }
    },
    "storageSKU": {
      "type": "string",
      "defaultValue": "Standard_LRS",
      "allowedValues": [
        "Standard_LRS",
        "Standard_GRS",
        "Standard_RAGRS",
        "Standard_ZRS",
        "Premium_LRS",
        "Premium_ZRS",
        "Standard_GZRS",
        "Standard_RAGZRS"
      ]
    }
  },
  "functions": [],
  "variables": {},
  "resources": [
    {
      "type": "Microsoft.Storage/storageAccounts",
      "apiVersion": "2025-01-01",
      "name": "[parameters('storageName')]",
      "tags": {
        "displayName": "[parameters('storageName')]"
      },
      "location": "[resourceGroup().location]",
      "kind": "StorageV2",
      "sku": {
        "name": "[parameters('storageSKU')]"
      }
    }
  ],
  "outputs": {}
}
```


[8] Deploy the ARM template
Deploy the template by running the following commands. Fill in a unique name for the storageName parameter. It must be globally unique across Azure
```pwsh
templateFile="azuredeploy.json"
today=$(date +"%d-%b-%Y")
DeploymentName="addSkuParameter-"$today

az deployment group create --name $DeploymentName --template-file $templateFile --parameters storageSKU=Standard_GRS storageName={your-unique-name}
```

[9] Run the following commands to deploy the template with a parameter that isn't allowed. Here, you changed the storageSKU parameter to Basic.
```pwsh
templateFile="azuredeploy.json"
today=$(date +"%d-%b-%Y")
DeploymentName="addSkuParameter-"$today

az deployment group create --name $DeploymentName --template-file $templateFile --parameters storageSKU=Basic storageName={your-unique-name}
```

[10] Add output to the ARM template
```json
"outputs": {
  "storageEndpoint": {
    "type": "object",
    "value": "[reference(parameters('storageName')).primaryEndpoints]"
  }
}
```
Save the file.



