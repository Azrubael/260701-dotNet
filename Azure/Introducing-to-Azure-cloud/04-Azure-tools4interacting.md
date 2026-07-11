### 2026-07-11
-------------------

1. What service helps you manage your Azure, on-premises, and multicloud environments?
Azure ARC

2. What two components could you use to implement an "infrastructure as code" deployment?
Bicep and ARM Templates

3. Which is the recommendations categories for Azure Advisor?
- *Reliability*
- *Security*
- *Performance*
- *Operational Excellence*
- *Cost*

4. You receive an email notification that virtual machines (VMs) in an Azure region where you have VMs deployed is experiencing an outage. Which component of Azure Service Health will let you know if your application is impacted?
Resource health


Azure tools for *managing your environment* , including the:
- Azure portal
	The Azure portal is a web-based, unified console that provides an alternative to command-line tools.
- Azure PowerShell
- Azure Command Line Interface (CLI)
	While Azure PowerShell uses PowerShell commands, the Azure CLI uses Bash commands.
- Cloud Shell
	Azure Cloud Shell is a browser-based shell tool that allows you to create, configure, and manage Azure resources using a shell. Azure Cloud Shell support both Azure PowerShell and the Azure Command Line Interface (CLI), which is a Bash shell.
- Copilot in Azure

`Azure Arc` works with Azure Resource Manager to extend your Azure compliance and monitoring to hybrid and multicloud configurations. It provides a centralized, unified way to:
+ Manage your entire environment together by projecting your existing non-Azure resources into Azure Resource Manager.
+ Manage multicloud and hybrid virtual machines, Kubernetes clusters, and databases as if they are running in Azure.
+ Use familiar Azure services and management capabilities, regardless of where they live.
+ Continue using traditional ITOps while introducing DevOps practices to support new cloud and native patterns in your environment.
+ Configure custom locations as an abstraction layer on top of Azure Arc-enabled Kubernetes clusters and cluster extensions.

В контексте Azure ITOps обычно включает:
- развертывание и сопровождение ресурсов Azure;
- мониторинг и алерты;
- управление конфигурациями и изменениями;
- безопасность и доступы;
- резервное копирование и восстановление;
- автоматизацию рутинных операций.

*Azure Resource Manager* is the deployment and management service for Azure. It provides a management layer that enables you to create, update, and delete resources in your Azure account.
Currently, Azure Arc allows you to manage the following resource types hosted outside of Azure:
- Servers
- Kubernetes clusters
- Azure data services
- SQL Server
- Virtual machines (preview)
A *Resource Manager* template is a JSON file that defines what you want to deploy to Azure so you can manage your infrastructure through declarative templates rather than scripts.
*Bicep* is a declarative language for deploying Azure resources through ARM. Compared to JSON ARM templates, Bicep is generally simpler and more concise.

`Azure Resource Manager` (ARM) — это платформа управления ресурсами Azure и одновременно JSON-шаблоны, которые Bicep компилирует в ARM templates.
`Azure Bicep` — это язык для описания и развертывания инфраструктуры Azure как кода. Он создан как более простой и удобный способ писать шаблоны для Azure.

`ARM` — низкоуровневый формат развертывания в JSON.
`Bicep` — более удобная надстройка над ARM для написания тех же развертываний.

Главные отличия:
+ Синтаксис: Bicep короче и читабельнее, чем JSON ARM.
+ Переиспользование: в Bicep проще работать с модулями.
+ Поддержка Azure: оба разворачивают ресурсы Azure через ARM, но Bicep удобнее для автора.
+ Компиляция: Bicep не заменяет ARM как механизм выполнения — он переводится в ARM-шаблон.


`Azure Advisor` evaluates your Azure resources and makes recommendations to help you improve reliability, security, performance, and cost efficiency. Recommendations fall into five categories:
- *Reliability* helps keep your applications running by flagging configuration risks.
- *Security* detects threats and vulnerabilities that could lead to breaches.
- *Performance* identifies changes that can speed up your applications.
- *Operational Excellence* suggests workflow and deployment improvements.
- *Cost* finds ways to reduce your Azure spending.

`Azure Service Health` helps you stay informed about the health of Azure itself and the specific resources you run. It combines three views that narrow in scope from global down to individual resources:
+ *Azure Status* gives you a global picture of Azure health across all services and regions. 
+ *Service Health* focuses on the Azure services and regions you actually use.
+ *Resource Health* zooms in on individual resources, such as a specific virtual machine. It tells you whether a resource is running normally or experiencing a problem, and whether the issue is on Azure's side or yours.

`Azure Monitor` gathers logs and metrics from your applications, operating systems, and network layers, stores that data centrally, and makes it available through dashboards, queries, and alerts.

`Log Analytics` is the tool in the Azure portal where you write and run queries against the data Azure Monitor collects. You can do simple filtering, like finding all errors in the last hour, or run advanced analytics to visualize trends over time.

`Azure Monitor Alerts` - notify you when Azure Monitor detects that a condition you defined has been met. You create an alert rule that specifies the condition, and an action group that controls who gets notified and what happens next.
Alerts can be metric-based or log-based. For example, a metric alert can send you an email when a VM's CPU stays above 80%, while a log alert can watch for a specific error pattern across multiple resources. Action groups are reusable across Azure Monitor, Service Health, and Azure Advisor.

`Application Insights` is an Azure Monitor feature that monitors the performance and usage of your web applications, whether they run in Azure, on-premises, or in another cloud. You can set up `Application Insights` by adding an SDK to your application code or by enabling the Application Insights agent without code changes.