# 2026-07-12
----------------

**Describe Azure virtual networking**

`Azure virtual networks` and `virtual subnets` enable Azure resources, such as VMs, web apps, and databases, to communicate with each other, with users on the internet, and with your on-premises client computers.

*Azure virtual networking* supports both `public` and `private` endpoints to enable communication between external or internal resources with other internal resources.
+ 	`Public endpoints` have a public IP address and can be accessed from anywhere in the world.
+ `Private endpoints` exist within a virtual network and have a private IP address from within the address space of that virtual network.

 When you set up a virtual network, you define a `private IP address` space by using either public or private `IP address ranges`. The IP range only exists within the virtual network and isn't internet routable. You can divide that IP address space into subnets and allocate part of the defined address space to each named subnet.
 
 For `name resolution`, you can use the name resolution service built into Azure. You also can configure the virtual network to use either an internal or an external DNS server.
 
 You can enable incoming connections from the internet by assigning a public IP address to an Azure resource, or putting the resource behind a public load balancer.
 
*Azure resources* can communicate securely with each other in one of two ways:
- `Virtual networks` can connect not only VMs but other Azure resources, such as the App Service Environment for Power Apps, Azure Kubernetes Service, and Azure virtual machine scale sets.
- `Service endpoints` can connect to other Azure resource types, such as Azure SQL databases and storage accounts. This approach lets you link multiple Azure resources to virtual networks to improve security and provide optimal routing between resources.


*You can create a network* that spans both your `local` and `cloud` environments in the three ways:
+ `Point-to-site virtual private network connections` are from a computer outside your environment back into your private network. In this case, the client computer initiates an encrypted VPN connection to connect to the Azure virtual network.
+ Виртуальные частные сети типа «точка-сайт» — это подключения компьютера, находящегося вне вашей среды, к вашей частной сети. В этом случае клиентский компьютер инициирует зашифрованное VPN-соединение для подключения к виртуальной сети Azure.

+ `Site-to-site virtual private networks` link your on-premises VPN device or gateway to the Azure VPN gateway in a virtual network. In effect, the devices in Azure can appear as being on the local network. The connection is encrypted and works over the internet.
+ Виртуальные частные сети типа «сайт-сайт» связывают ваше локальное VPN-устройство или шлюз с VPN-шлюзом Azure в виртуальной сети. По сути, устройства в Azure могут отображаться как находящиеся в локальной сети. Соединение зашифровано и работает через Интернет.

+ `Azure ExpressRoute` provides a dedicated private connectivity to Azure that doesn't travel over the internet. ExpressRoute is useful for environments where you need greater bandwidth and even higher levels of security.
+ Azure ExpressRoute предоставляет выделенное частное подключение к Azure, которое не проходит через Интернет. ExpressRoute полезен для сред, где требуется большая пропускная способность и еще более высокий уровень безопасности.


By default, *Azure routes traffic between subnets* on any connected virtual networks, on-premises networks, and the internet. You also can control routing and override those settings, as follows:
- `Route tables` let you define rules about how traffic should be directed. You can create custom route tables that control how packets are routed between subnets.
- `Border Gateway Protocol` (BGP) works with Azure VPN gateways, Azure Route Server, or Azure ExpressRoute to propagate on-premises BGP routes to Azure virtual networks.
- `User-defined routes` (UDR) let you control the routing tables between subnets within a virtual network or between virtual networks, giving you greater control over network traffic flow.


*Azure virtual networks* let you filter `traffic` between `subnets` by using the following approaches:
+ `Network security` groups are Azure resources that can contain multiple inbound and outbound security rules. You can define these rules to allow or block traffic, based on factors such as source and destination IP address, port, and protocol.

+ `Network virtual` appliances are specialized VMs that can be compared to a hardened network appliance. A network virtual appliance carries out a particular network function, such as running a firewall or performing wide area network (WAN) optimization.
+ Виртуальные сетевые устройства — это специализированные виртуальные машины, которые можно сравнить с защищенным сетевым устройством. Виртуальное сетевое устройство выполняет определенную сетевую функцию, например, запускает межсетевой экран или выполняет оптимизацию глобальной сети (WAN).

**Connect virtual networks**
You can link `virtual networks` together by using `virtual network peering`. Peering allows two virtual networks to connect directly to each other. Network traffic between peered networks is private, and travels on the Microsoft backbone network, never entering the public internet. Peering enables resources in each virtual network to communicate with each other. These virtual networks can be in separate regions, which lets you create a global interconnected network through Azure.

A `virtual private network` (VPN) uses an encrypted tunnel within another network. VPNs are typically deployed to connect two or more trusted private networks to one another over an untrusted network (typically the public internet). Traffic is encrypted while traveling over the untrusted network to prevent eavesdropping or other attacks. VPNs can enable networks to safely and securely share sensitive information.


**Azure VPN gateways**:
- Connect on-premises datacenters to virtual networks through a site-to-site connection.
- Connect individual devices to virtual networks through a point-to-site connection.
- Connect virtual networks to other virtual networks through a network-to-network connection.
 You can deploy only `one VPN gateway` in each virtual network. However, you can use one gateway to connect to multiple locations, which includes other virtual networks or on-premises datacenters.
 
 *When setting up a `VPN gateway,` you must specify the `type of VPN` - either policy-based or route-based*:
+ `Policy-based VPN gateways` specify statically the IP address of packets that should be encrypted through each tunnel. This type of device evaluates every data packet against those sets of IP addresses to choose the tunnel through which that packet is sent.
+ `VPN-шлюзы на основе политик` статически задают IP-адрес пакетов, которые должны быть зашифрованы через каждый туннель. Устройство такого типа оценивает каждый пакет данных по этим наборам IP-адресов, чтобы выбрать туннель, через который этот пакет будет отправлен.

+ In `route-based gateways`, IPSec tunnels are modeled as a network interface or virtual tunnel interface. IP routing (either static routes or dynamic routing protocols) decides which one of these tunnel interfaces to use when sending each packet. Route-based VPNs are the preferred connection method for on-premises devices. They're more resilient to topology changes such as the creation of new subnets.
Use a `route-based VPN gateway` if you need any of the following types of connectivity:
	a)  Connections between virtual networks
	b) Point-to-site connections
	c) Multisite connections
	d) Coexistence with an Azure ExpressRoute gateway

+ `В шлюзах на основе маршрутизации`, туннели IPSec моделируются как сетевой интерфейс или виртуальный интерфейс туннеля. IP-маршрутизация (статические маршруты или динамические протоколы маршрутизации) определяет, какой из этих интерфейсов туннеля использовать при отправке каждого пакета. VPN-сети на основе маршрутизации являются предпочтительным методом подключения для локальных устройств. Они более устойчивы к изменениям топологии, таким как создание новых подсетей.


**High-availability scenarios**
There are a few ways to maximize the resiliency of your VPN gateway:
- `Active/standby`. By default, VPN gateways are deployed as two instances in an active/standby configuration, even if you only see one VPN gateway resource in Azure.
- `Active/active`. In this configuration, you assign a unique public IP address to each instance. You then create separate tunnels from the on-premises device to each IP address. You can extend the high availability by deploying an additional VPN device on-premises.
- `ExpressRoute failover`. ExpressRoute circuits have resiliency built in. However, they aren't immune to physical problems that affect the cables delivering connectivity or outages that affect the complete ExpressRoute location. In high-availability scenarios, you can also provision a VPN gateway that uses the internet as an alternative method of connectivity.
- `Zone-redundant gateways` in regions that support availability zones. This configuration brings resiliency, scalability, and higher availability to virtual network gateways. Deploying gateways in Azure availability zones physically and logically separates gateways within a region while protecting your on-premises network connectivity to Azure from zone-level failures. These gateways require different gateway stock keeping units (SKUs) and use Standard public IP addresses instead of Basic public IP addresses.

**Describe Azure ExpressRoute**
Azure ExpressRoute lets you extend your on-premises networks into the Microsoft cloud over a private connection, with the help of a connectivity provider. This connection is called an ExpressRoute Circuit. With ExpressRoute, you can establish connections to Microsoft cloud services, such as Microsoft Azure and Microsoft 365. ExpressRoute lets you connect offices, datacenters, or other facilities to the Microsoft cloud. Each location would have its own ExpressRoute circuit.
Connectivity can be from an any-to-any (IP VPN) network, a point-to-point Ethernet network, or a virtual cross-connection through a connectivity provider at a colocation facility. ExpressRoute connections don't go over the public internet so they offer more reliability, faster speeds, consistent latencies, and higher security than typical internet connections.

There are several `benefits to using ExpressRoute `as the connection service between Azure and on-premises networks.
+ Connectivity to Microsoft cloud services across all regions in the geopolitical region.
+ Global connectivity to Microsoft services across all regions with the ExpressRoute Global Reach.
+ Dynamic routing between your network and Microsoft via Border Gateway Protocol (BGP).
+ Built-in redundancy in every peering location for higher reliability.

*ExpressRoute enables direct access to the following services in all regions*:
- Microsoft Office 365
- Microsoft Dynamics 365
- Azure compute services, such as Azure Virtual Machines
- Azure cloud services, such as Azure Cosmos DB and Azure Storage

You can enable *ExpressRoute Global Reach* to exchange data across your on-premises sites by connecting your `ExpressRoute circuits`. For example, suppose you have an office in Asia and a datacenter in Europe, both with ExpressRoute circuits connecting them to the Microsoft network. You can use ExpressRoute Global Reach to connect those two facilities, allowing them to communicate without transferring data over the public internet.

At a high level, choose *ExpressRoute* when:
+ You need private, consistent connectivity between on-premises networks and Azure.
+ Your team has strict compliance or data-transfer requirements.
+ You need predictable latency and high-throughput network performance.
+ You want to avoid sending critical traffic over the public internet.

With `ExpressRoute`, your data doesn't travel over the public internet, reducing the risks associated with internet communications. ExpressRoute is a private connection from your on-premises infrastructure to your Azure infrastructure. 
