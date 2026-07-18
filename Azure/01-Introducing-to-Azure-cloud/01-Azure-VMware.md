## How Azure VMware Solution works

1. What component of Azure VMware Solution is the customer not responsible for?
Microsoft, in partnership with VMware, supports and maintains the lifecycle management of VMware software (ESXi, vCenter, and vSAN).

2. Shannon wants to start migrating to Azure VMware Solution. What minimum number of nodes must Shannon have to get started?
The minimum number of nodes required in a cluster is three.

3. What does the CloudAdmin role in the Azure VMware Solution vCenter Server do?
The CloudAdmin role creates and manages workloads in your private cloud.

4. Which type of scaling involves adding or removing resources (such as virtual machines or containers) to meet demand?
Horizontal scaling

5. What is characterized as the ability of a system to recover from failures and continue to function?
Reliability

6. Which cloud practice most directly supports sustainability by reducing unnecessary resource use?
Automatically scaling down resources when demand drops

7. Which cloud service type is most suited to a lift and shift migration from an on-premises datacenter to a cloud deployment?
IaaS

8. What type of cloud service type would a Finance and Expense tracking solution typically be in?
SaaS


SLA: service-level agreements:
99% - 3.65 downtime days / year
99.9% - 8.76 downtime hours / year
99.99% - 52.6 downtime minutes / year

Scalability refers to the ability to adjust resources to meet demand. 
Vertical Scaling: add CPU/RAM
Horizontal Scaling: Add more instances

Reliability - recover from failures, pillar of the Azure Well-Architectured Framework
Predictability - influenced by the Azure Well-Architected Framework (predict resources needed, spend in advance and performance)

Security and Governance in the cloud:
- Governance - set standarts and update at scale;
- Compliance - audit and remediate, patches & maintenance;
- Security - match your security needs, built-in protestions.

Manageability in the cloud (with Web Portal, CLI, APIs, PowerShell)
- Automatically scale resource deployment based on need.
- Deploy resources based on a preconfigured template, removing the need for manual configuration.
- Monitor the health of resources and automatically replace failing resources.
- Receive automatic alerts based on configured metrics, so you're aware of performance in real time.

Sustainability in the cloud:
- Match resources to actual demand;
- Scale down and shut off unused resources;
- Choose efficient services and configurations;
- Track resource usage and spending by using governance and monitoring.

**Infrastructure as a service** (IaaS) is the most flexible category of cloud services. A customer responsible for:
- Operating system installation, configuration, and maintenance
- Network configuration
- Database and storage configuration
In IaaS, the cloud provider is responsible for the physical infrastructure and internet connectivity.
Common scenarios where IaaS might make sense:
+ Lift-and-shift migration
+ Testing and development

**Platform as a service** (PaaS), you don't have to worry about the licensing or patching for operating systems and databases.
Common scenarios where PaaS might make sense:
+ Development framework
+ Analytics or business intelligence

**Software as a service** (SaaS) is the most complete cloud serviceю
Common scenarios for SaaS include:
+ Email and messaging.
+ Productivity applications.
+ Finance and expense tracking.
