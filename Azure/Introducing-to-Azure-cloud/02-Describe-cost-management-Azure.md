**Factors that can affect costs in Azure**

1. What Azure feature can help stay organized and track usage based on metadata associated with resources?
tags

2. Which tool helps estimate the cost of deploying Azure services before implementation?
Azure pricing calculator

3. You have a workload that can tolerate interruptions and should run at the lowest possible compute cost. Which option is typically the best fit?
Spot Virtual Machines


Azure pricing calculator
https://azure.microsoft.com/en-us/pricing/calculator/

# Factors that can affect costs in Azure
- Resource type
	Type, settings, region
- Consumption
	Pay-as-you-go or commit for discounts
- Maintenance
	Unused resources still cost money
- Geography
	Region and data transfer zones
- Subscription type
	Free trials and usage allowances vary
- Azure Marketplace
	Third party vendor costs added on top
	
All solutions available in Azure Marketplace are certified and compliant with Azure policies and standards.

# The three types of alerts that may show up are:
- Budget alerts
	Notify you when spending reaches or exceeds a threshold you define.
- Credit alerts
	Notify you when your Azure Prepayment (previously called monetary commitment) is consumed.
- Department spending quota alerts.
	Are specific to Enterprise Agreement customers. They notify you when department spending reaches a fixed threshold of the quota. 

A *budget* is where you set a spending limit for Azure. You can set *budgets* based on a subscription, resource group, service type, or other criteria. You can also configure budgets to trigger automation that suspends or modifies resources when a spending threshold is reached.

# The purpose of tags
**Resource management** Tags enable you to locate and act on resources that are associated with specific workloads, environments, teams, and owners.
**Cost management** and optimization Tags enable you to group resources so that you can report on costs, allocate internal cost centers, track budgets, and forecast estimated cost.
**Operations management** Tags enable you to group resources according to how critical their availability is to your operations. This grouping helps you formulate service-level agreements (SLAs). An SLA is an uptime or performance guarantee between you and your users.
**Security** Tags enable you to classify data by its security level, such as public or confidential.
**Governance and regulatory compliance** Tags enable you to identify resources that align with governance or regulatory compliance requirements, such as ISO 27001. Tags can also be part of your standards enforcement efforts. For example, you might require that all resources be tagged with an owner or department name.
**Workload optimization and automation** Tags can help you visualize all of the resources that participate in complex deployments. For example, you might tag a resource with its associated workload or application name and use software such as Azure DevOps to perform automated tasks on those resources.

You can add, modify, or delete resource tags through the Azure portal, PowerShell, the Azure CLI, Azure Resource Manager templates, or the REST API.

Choose **Reservations** for predictable, long-running workloads with stable resource needs.
Choose Azure **savings plan** for compute when usage is steady but you need more flexibility across compute services.
Choose **Spot pricing** for fault-tolerant or interruptible workloads where lowest cost is the top priority.