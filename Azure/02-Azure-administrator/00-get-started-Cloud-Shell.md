### 2026-07-15
------------------

**Get started with Azure Cloud Shell**
https://learn.microsoft.com/en-us/azure/cloud-shell/get-started/classic?tabs=azurecli

1. Sign in to the Azure portal.
2. On the Azure portal menu, search for Subscriptions. Select it from the available options.
3. On the Subscriptions page, select your subscription.
4. On your subscription page, expand Settings in left menu and select Resource providers.
5. In the Filter by name... box, enter cloudshell to search for the resource provider.
6. Select the Microsoft.CloudShell resource provider from the provider list.
7. Select Register to change the status from unregistered to Registered.
8. Launch Cloud Shell from the top navigation of the Azure portal.
9. The first time you start Cloud Shell you're prompted to create an Azure Storage account for the Azure file share. Select the Subscription used to create the storage account and file share.

10. List subscriptions you have access to.
PS > az account list
11. Set your preferred subscription:
PS > az account set --subscription 'my-subscription-name'

Your subscription is remembered for future sessions using /home/<user>/.azure/azureProfile.json.

12. Get a list of Azure commands
```pwsh
# Run the following command to see a list of all Azure CLI commands.
PS > az
# Run the following command to get a list of Azure CLI commands that apply to WebApps:
PS > az webapp --help
```

**Azure PowerShell documentation**
https://learn.microsoft.com/en-us/powershell/azure/?view=azps-16.1.0