### 2026-07-11
------------------

**QUIZ**
1. How many resource groups can a resource be in at the same time?
One

2. What happens to the resources within a resource group when an action or setting at the Resource Group level is applied?
The setting is applied to current and future resources.

3. What Azure feature replicates resources across at least 300 miles away from each other?
Region pairs.



The Azure free account includes:
- Free access to popular Azure products for 12 months.
- A credit to use for the first 30 days.
- Access to more than 65 services that are always free.

To sign up, you need a phone number, a credit card, and a Microsoft or GitHub account.

The `physical infrastructure` for Azure starts with `datacenters`. These datacenters are facilities with servers arranged in racks, with dedicated power, cooling, and networking infrastructure — similar to an on-premises datacenter, but at a much larger scale.

*Availability zones* are physically separate datacenters within an *Azure region*. Each availability zone is made up of one or more datacenters equipped with independent power, cooling, and networking. An availability zone is set up to be an isolation boundary. If one zone goes down, the other continues working. Availability zones are connected through high-speed, private fiber-optic networks.

To ensure resiliency, a minimum of three separate availability zones are present in all availability zone-enabled regions. However, not all Azure Regions currently support `availability zones`.

A *resource* is the basic building block of Azure. Anything you create, provision, or deploy is a resource. VMs, virtual networks, databases, and Azure AI services are all examples of resources.
*Resource groups* are groupings of resources. Every resource must belong to exactly one resource group. You can move some resources between groups, but a resource is only associated with one group at a time.
In Azure, *subscriptions* are a unit of management, billing, and scale. *Subscriptions* let you organize resource groups and control billing separately from access. There are two types of subscription boundaries: `billing` boundary; `access control` boundary.

Resources go into resource groups, and resource groups go into subscriptions. For a small environment, that's enough.  Azure *management groups* sit above subscriptions.
You can build a flexible structure of *management groups* and *subscriptions* to organize your resources into a hierarchy for unified policy and access management.

Examples of how you could use *management groups*:
- Apply a policy across subscriptions
- Grant access to multiple subscriptions at once


Here’s a concept map (relationships) tying those Azure-style concepts together:
[Management Group]
   └─(contains)
      [Subscription]
         └─(contains)
            [Resource Group]
               └─(contains)
                  [Resources]
                     ├─(deployed to)
                     │   [Region]
                     │      ├─(contains)
                     │      │   [Datacenters]
                     │      │      └─(host)
                     │      │         [Compute/Storage/Network infrastructure]
                     │      └─(grouping within)
                     │         [Availability Zones]
                     │            └─(each ZA is implemented across
                     │               datacenters with independence)
                     └─(uses/depends on)
                        [Zone-specific capacity] (if zone-redundant/zone-pinned)
