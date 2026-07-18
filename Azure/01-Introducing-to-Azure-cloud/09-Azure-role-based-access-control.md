## 2026-07-13
-----------------

**Azure role-based access control**
-----------------------------------------
The principle of `least privilege` says you should only grant access up to the level needed to complete a task.
Instead of defining the detailed access requirements for each individual, and then updating access requirements when new resources are created or new people join the team, Azure enables you to control access through Azure role-based access control (Azure RBAC).
Azure provides built-in roles that describe common access rules for cloud resources. You can also define your own roles. Each role has an associated set of access permissions that relate to that role.

Role-based access control is applied to a scope, which is a resource or set of resources that this access applies to. Scopes include:
    A management group (a collection of multiple subscriptions).
    A single subscription.
    A resource group.
    A single resource.
Управление доступом на основе ролей применяется к области действия, которая представляет собой ресурс или набор ресурсов, к которым применяется данный доступ.

Azure RBAC is hierarchical, in that when you grant access at a parent scope, those permissions are inherited by all child scopes. For example:
+ When you assign the Owner role to a user at the management group scope, that user can manage everything in all subscriptions within the management group.
+ When you assign the Reader role to a group at the subscription scope, the members of that group can view every resource group and resource within the subscription.

*How is Azure RBAC enforced?*
Azure RBAC doesn't enforce access permissions at the application or data level. Application security must be handled by your application.
Azure RBAC uses an allow model. When you're assigned a role, Azure RBAC allows you to perform actions within the scope of that role. If one role assignment grants you read permissions to a resource group and a different role assignment grants you write permissions to the same resource group, you have both read and write permissions on that resource group.
[Как обеспечивается контроль доступа на основе ролей (RBAC) в Azure?
Azure RBAC не обеспечивает принудительное управление правами доступа на уровне приложения или данных. Безопасность приложения должна обеспечиваться самим приложением.ї]


**Zero Trust model**
[В основе принципа доверия лежит предположение о нарушении безопасности на начальном этапе, после чего каждый запрос проверяется так, как если бы он исходил из неконтролируемой сети.]
Microsoft highly recommends the `Zero Trust` security model:
- `Verify explicitly` - Always authenticate and authorize based on all available data points.
- `Use least privilege access` - Limit user access with Just-In-Time and Just-Enough-Access (JIT/JEA), risk-based adaptive policies, and data protection.
- `Assume breach` - Limit the potential impact and segment access. Verify end-to-end encryption. Use analytics to get visibility, drive threat detection, and improve defenses.

Instead of assuming that a device is safe because it’s within a trusted internal network, it requires everyone to authenticate. Then grants access based on authentication rather than location.


**Defense-in-depth**
-------------------------
A defense-in-depth strategy uses a series of mechanisms to slow the advance of an attack that aims at acquiring unauthorized access to data.
You can visualize defense-in-depth as a set of layers, with the data to be secured at the center and all the other layers functioning to protect that central data layer.
*Here's a brief overview of the role of each layer*:
+ `The physical security layer` is the first line of defense to protect computing hardware in the datacenter.
+ `The identity and access layer` controls access to infrastructure and change control.
+ `The perimeter layer` uses distributed denial of service (DDoS) protection to filter large-scale attacks before they can cause a denial of service for users.
+ `The network layer` limits communication between resources through segmentation and access controls.
+ `The compute layer` secures access to virtual machines.
+ `The application layer` helps ensure that applications are secure and free of security vulnerabilities.
+ `The data layer` controls access to operational and customer data that you need to protect.

Each layer provides protection so that if one layer is breached, a subsequent layer is already in place to prevent further exposure. This approach removes reliance on any single layer of protection. It slows down an attack and provides alert information that security teams can act upon, either automatically or manually.


**Encryption and key management in Azure**
------------------------------------------------------
At a fundamentals level, remember that Azure Key Vault is the primary Azure service for secure key and secret storage.
Encryption helps protect data confidentiality by making data unreadable to unauthorized users.
In Azure, encryption is commonly discussed in two forms:
+ Encryption at rest protects data when it is stored, such as in databases, disks, and storage accounts.
+ Encryption in transit protects data while it moves between services, applications, and users.

`Azure Key Vault` is a service for securely storing and controlling access to:
- Secrets (such as connection strings and passwords)
- Encryption keys
- Certificates

Key management supports security and compliance goals by helping you:
+ Control who can access keys and secrets
+ Rotate and update keys over time
+ Audit key and secret usage

When applications retrieve secrets from Key Vault at runtime by using managed identities, teams can reduce hard-coded credentials and improve overall secret governance.


**Microsoft Defender for Cloud**
---------------------------------------
Defender for Cloud — это сервис для управления состоянием безопасности и защиты от угроз.
`Defender for Cloud` helps you harden resources, track risk, and respond to threats.
For hybrid and multicloud estates, `Defender for Cloud` extends visibility so security teams can work from one control plane.
When needed, Defender for Cloud can deploy data collection components for security insights. Azure Arc can extend Microsoft Defender plans to non-Azure machines, and Cloud Security Posture Management (CSPM) capabilities can assess multicloud resources agentlessly.
Defender for Cloud can also protect resources in other clouds, including AWS and GCP.
`Defender for Cloud` helps you continuously assess your environment. It includes vulnerability assessment for virtual machines, container registries, and SQL servers.
