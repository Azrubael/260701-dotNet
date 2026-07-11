QUIZ
1. How can you prevent creation of non-compliant resources, without having to manually evaluate each resource?
Azure policy

2. What's the best way to prevent inadvertent deletion of a resource?
Azure resource locks

Two main solution areas comprise *Microsoft Purview*: *risk and compliance* and *unified data governance*.
*Microsoft 365* features as a core component of the *Microsoft Purview* risk and compliance solutions.
*Microsoft Purview*, by managing and monitoring your data, helps your team to manage your data stored in _Azure_, _SQL_ and _Hive_ databases, locally, and even in other clouds like _Amazon S3_:
+ Protect sensitive data across clouds, apps, and devices.
+ Identify data risks and manage regulatory compliance requirements.
+ Get started with regulatory compliance.

Microsoft Purview unified data governance:
- Create an up-to-date map of your entire data estate that includes data classification and end-to-end lineage.
- Identify where sensitive data is stored in your estate.
- Create a secure environment for data consumers to find valuable data.
- Generate insights about how your data is stored and used.
- Manage access to the data in your estate securely and at scale.

*Azure Policy* enables you to define both individual policies and groups of related policies, known as *initiatives*. *Azure Policy* evaluates your resources and highlights resources that aren't compliant with the policies you've created. *Azure Policy* can also prevent noncompliant resources from being created.
*Azure Policies* can be set at each level, enabling you to set policies on a specific resource, resource group, subscription, and so on. Additionally, *Azure Policies* are inherited.
*Azure Policy* can automatically remediate noncompliant resources and configurations.

Enable Monitoring in Azure Security Center initiative contains over 100 separate policy definitions.

Even with Azure role-based access control (Azure RBAC) policies in place, there's still a risk that people with the right level of access could delete critical cloud resources. *Resource locks* (Delete or ReadOnly) prevent resources from being deleted or updated, depending on the type of lock.
To modify a locked resource, you must first remove the lock. After you remove the lock, you can apply any action you have permissions to perform. Resource locks apply regardless of RBAC permissions.

The *Service Trust Portal* contains details about Microsoft's implementation of controls and processes that protect our cloud services and customer data. To access some of the resources on the Service Trust Portal, you must sign in as an authenticated user with your Microsoft cloud services account
