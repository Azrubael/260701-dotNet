# 2026-07-13
----------------

`Microsoft Active Directory` (AD) — это служба каталогов, разработанная компанией Microsoft для управления сетевыми ресурсами в корпоративных сетях. Она предоставляет централизованное хранилище для информации о пользователях, компьютерах, группах, политиках безопасности и других объектах сети, а также обеспечивает аутентификацию и авторизацию доступа к ресурсам.

Microsoft Entra ID is Microsoft's cloud-based identity and access management service. It lets you sign in and access both Microsoft cloud applications and cloud applications that you develop.

*Microsoft Entra ID* is for:
- `IT administrators`. Administrators can use Microsoft Entra ID to control access to applications and resources based on workload and security requirements.
- `App developers`. Developers can use Microsoft Entra ID to provide a standards-based approach for adding functionality to applications that they build, such as adding SSO functionality to an app or enabling an app to work with a user's existing credentials.
- `Users`. Users can manage their identities and take maintenance actions like self-service password reset.
- `Online service subscribers`. Microsoft 365, Microsoft Office 365, Azure, and Microsoft Dynamics CRM. Online subscribers are already using Microsoft Entra ID to authenticate into their account.

*Microsoft Entra ID* provides services such as:
- `Authentication` — Verifies identity before granting access. Includes self-service password reset, multifactor authentication, banned password lists, and smart lockout.
- `Single sign-on (SSO)` — Lets one identity access multiple applications. SSO benefits and behavior are covered in the authentication methods unit.
- `Application management` — Manages cloud and on-premises apps through features like Application Proxy, SaaS app integration, and the My Apps portal.
- `Device management` — Supports device registration and management through tools like Microsoft Intune. Enables device-based Conditional Access policies that restrict access to known devices.

When you create a Microsoft Entra Domain Services managed domain, you define a unique namespace. This namespace is the domain name. Two Windows Server domain controllers are then deployed into your selected Azure region. This deployment is known as a replica set.

`Single sign-on` (SSO) lets a user sign in once and access multiple trusted applications. SSO reduces password sprawl, which lowers the chance of credential-related incidents and decreases account lockout and reset overhead.
Single sign-on is only as secure as the initial authenticator because the subsequent connections are all based on the security of the initial authenticator.

`Multifactor authentication` (MFA) prompts a user for an extra factor during sign-in, so a compromised password alone isn't enough for access. These factors fall into three categories:
- Something the user knows — a password or challenge question.
- Something the user has — a code sent to a mobile phone.
- Something the user is — a biometric signal such as a fingerprint or face scan.

Microsoft *Entra ID* supports three `passwordless` options:
+ Windows Hello for Business
+ Microsoft Authenticator app
+ FIDO2 security keys

Microsoft *Entra External ID* включает в себя возможности, используемые для безопасного взаимодействия с пользователями за пределами вашей клиентской сети.

The following capabilities make up External Identities:
- `B2B collaboration` - Collaborate with external users by letting them use their preferred identity to sign in to your Microsoft applications or other internal applications (SaaS apps, custom-developed apps, etc.). B2B collaboration users are represented in your directory, typically as guest users.
- `B2B direct connect` - Establish a mutual, two-way trust with another Microsoft Entra tenant for seamless collaboration. B2B direct connect currently supports Teams shared channels, enabling external users to access your resources from within their home instances of Teams. B2B direct connect users aren't represented in your directory, but they're visible from within the Teams shared channel and can be monitored in Teams admin center reports.
- `Microsoft Entra External ID` for customers (formerly Azure AD B2C) - Publish modern SaaS apps or custom-developed apps (excluding Microsoft apps) to consumers and customers, while using Entra External ID for identity and access management.

With *Microsoft Entra ID*, you can enable collaboration across tenant boundaries by using B2B features. Guest users from other tenants can be invited by administrators or authorized users. This capability also applies to social identities such as Microsoft accounts.

You also can easily ensure that guest users have appropriate access. You can ask the guests themselves or a decision maker to participate in an access review and recertify (or attest) to the guests' access. The reviewers can give their input on each user's need for continued access, based on suggestions from Microsoft Entra ID. When an access review is finished, you can then make changes and remove access for guests who no longer need it.


**Azure conditional access**
Conditional Access is a tool that Microsoft Entra ID uses to allow (or deny) access to resources based on identity signals. Conditional Access helps IT administrators:
- Empower users to be productive wherever and whenever.
- Protect critical assets.

`Conditional Access` also provides a more granular multifactor authentication experience for users. For example, a user might not be challenged for a second authentication factor if they're at a known location. However, they might be challenged for a second authentication factor if their sign-in signals are unusual or they're at an unexpected location.

`Conditional Access` is useful when you need to:
- Require multifactor authentication (MFA) to access an application depending on the requester’s role, location, or network. For example, you could require MFA for administrators, or for people connecting from outside trusted network locations.
- Require access to services only through approved client applications. For example, you could limit which email applications are able to connect to your email service.
- Require users to access your application only from managed devices. A managed device is a device that meets your standards for security and compliance.
- Block access from untrusted sources, such as access from unknown or unexpected locations.

Common IT tasks include requiring MFA for privileged roles, restricting access to sensitive apps from unmanaged devices, and blocking risky sign-ins from unexpected locations.


