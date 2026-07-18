### 2026-07-15
------------------

**QUIZ**
1. Which of these methods can you use to access Azure Cloud Shell from a computer running Windows 11?
Use the Microsoft Edge to log into an Azure Subscription

2. Which of these methods can you use to access Azure Cloud Shell from a computer running Windows 11?
Upload the script to your CloudDrive on an Azure Cloud Shell session.

3. You have a script stored on the Cloud Shell storage. You constantly use this script for resource management, but you need to perform small changes to it. Which of these solutions is the best way to handle the situation?
Use the Cloud Shell editor to make the necessary changes and save it directly on the CloudDrive.


`Azure Cloud Shell` is a command-line environment you can access through your web browser. You can use this environment to manage Azure resources, including VMs, storage, and networking. Just like you do when using the `Azure CLI` or `Azure PowerShell`.

Azure PowerShell command to call:
PS > Get-AzSubscription

Azure PowerShell command example:
PS > Get-AzVM -Name DC01

*You can use Azure Cloud Shell to*:
- Open a secure command-line session from any browser-based device.
- Interact with Azure resources without the need to install plug-ins or add-ons to your device.
- Persist files between sessions for later use.
- Use either Bash or PowerShell, whichever you prefer, to manage Azure resources.
- Edit files (such as scripts) via the Cloud Shell editor.

*You shouldn't use Azure Cloud Shell if*:
+ You intend to leave a session open for more than 20 minutes for long running scripts or activities. In these cases, your session is disconnected without warning, and the current state is lost.
+ You need admin permissions, such as sudo access, from within the Azure CLI or PowerShell environment.
+ You need to install tools that aren't supported in the limited Cloud Shell environment, but instead require an environment such as a custom virtual machine or container.
+ You need storage from different regions. You might need to back up and synchronize this content since only one region can have the storage allocated to Azure Cloud Shell.
+ You need to open multiple sessions at the same time. Azure Cloud Shell allows only one instance at time and isn't suitable for concurrent work across multiple subscriptions or tenants.


https://learn.microsoft.com/en-us/powershell/azure/?view=azps-16.1.0
https://learn.microsoft.com/en-us/azure/cloud-shell/features#preinstalled-tools

The most commonly used tools are preinstalled in `Cloud Shell.` This curated collection of tools is updated monthly. Use the following commands to see the current list of tools and versions.
+ In PowerShell, use the `Get-Module -ListAvailable` command to get a list of installed module
In Bash or PowerShell
	Use the `tdnf list` command to list the TDNF packages that are installed
	Use the `pip3 list` command to list the Python packages that are installed
