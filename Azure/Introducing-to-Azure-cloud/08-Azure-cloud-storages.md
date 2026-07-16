## 2026-07-13
-----------------

**Azure storage accounts**


**QUIZ**
----------
1. Which tool automatically keeps files between an on-premises Windows server and an Azure cloud environment updated?
Azure File Sync

2. Which storage redundancy option provides the highest degree of durability, with 16 nines of durability?
Geo-zone-redundant-storage

3. Which Azure Storage service supports big data analytics, as well as handling text and binary data types?
Azure blobs


A storage account provides a unique namespace for your *Azure Storage* data that's accessible from anywhere in the world over HTTP or HTTPS. Data in this account is secure, highly available, durable, and massively scalable.
These redundancy options are covered later in this module:
- Locally redundant storage (LRS)
- Geo-redundant storage (GRS)
- Read-access geo-redundant storage (RA-GRS)
- Zone-redundant storage (ZRS)
- Geo-zone-redundant storage (GZRS)
- Read-access geo-zone-redundant storage (RA-GZRS)

When selecting a redundancy option, balance lower cost against higher availability. Key decision factors include:
+ How your data is replicated in the primary region.
+ Whether your data is replicated to a second region that is geographically distant to the primary region, to protect against regional disasters.
+ Whether your application requires read access to the replicated data in the secondary region if the primary region becomes unavailable.

Every storage account must have a unique account name within Azure. The combination of the account name and the Azure Storage service endpoint forms the endpoints for your storage account.

Data in `Azure Storage` is always replicated three times in the primary region. Primary-region options are locally redundant storage (LRS) and zone-redundant storage (ZRS).
LRS replicates your data three times within a single data center in the primary region. LRS provides at least 11 nines of durability (99.999999999%) of objects over a given year.
Because all replicas stay in that datacenter, a datacenter-level disaster can still cause data loss. For stronger resilience, use ZRS, GRS, or GZRS.

With ZRS, data stays available for read and write operations even if one zone is unavailable. Azure performs networking updates, such as DNS repointing, during recovery.
Microsoft recommends ZRS for high availability in-region scenarios and for requirements that keep replication within a country or region.

For higher durability, you can replicate data to a secondary region that is geographically distant from the primary region.
When you create a storage account, you choose the primary region. Azure assigns the paired secondary region based on region pairs.
Secondary-region options are geo-redundant storage (GRS) and geo-zone-redundant storage (GZRS). GRS uses LRS in both regions, while GZRS uses ZRS in the primary region and LRS in the secondary region.

[Because data is replicated to the secondary region asynchronously, a failure that affects the primary region may result in data loss if the primary region can't be recovered. The interval between the most recent writes to the primary region and the last write to the secondary region is known as the recovery point objective (RPO). The RPO indicates the point in time to which data can be recovered. Azure Storage typically has an RPO of less than 15 minutes, although there's currently no SLA on how long it takes to replicate data to the secondary region.]

GRS copies data `synchronously` three times in the primary region (LRS), then `asynchronously` to the secondary region (also LRS). It provides at least 16 nines of durability over a year.
GZRS is designed to provide at least 16 nines (99.99999999999999%) of durability of objects over a given year.

GRS and GZRS protect against regional outages by replicating data to a secondary location. To read secondary-region data before failover, enable read-access geo-redundant storage (RA-GRS) or read-access geo-zone-redundant storage (RA-GZRS).
Remember that the data in your secondary region may not be up-to-date due to RPO.

*Azure Storage includes these core data services*:
+ Azure Blobs: A massively scalable object store for text and binary data. Also includes support for big data analytics through Data Lake Storage Gen2.
+ Azure Files: Managed file shares for cloud or on-premises deployments.
+ Azure Queues: A messaging store for reliable messaging between application components.
+ Azure Disks: Block-level storage volumes for Azure VMs.
+ Azure Tables: NoSQL table option for structured, non-relational data.

Benefits of Azure Storage
- `Durable and highly available`. Redundancy options protect data from hardware failures, outages, and regional incidents.
- `Secure`. All data written to an Azure storage account is encrypted by the service. Azure Storage provides you with fine-grained control over who has access to your data.
- `Scalable`. Azure Storage is designed to be massively scalable to meet the data storage and performance needs of today's applications.
- `Managed`. Azure handles hardware maintenance, updates, and critical issues for you.
- `Accessible`. Data can be accessed globally over HTTP or HTTPS by using REST APIs, SDKs, Azure CLI, Azure PowerShell, the Azure portal, or Azure Storage Explorer.

**Blob storage** is ideal for:
+ Serving images or documents directly to a browser.
+ Storing files for distributed access.
+ Streaming video and audio.
+ Storing data for backup and restore, disaster recovery, and archiving.
+ Storing data for analysis by an on-premises or Azure-hosted service.

Objects in blob storage can be accessed from anywhere in the world via HTTP or HTTPS. Users or client applications can access blobs via URLs, the Azure Storage REST API, Azure PowerShell, Azure CLI, or an Azure Storage client library.

**Azure Files** offers fully managed file shares in the cloud that are accessible via the industry standard Server Message Block (SMB) or Network File System (NFS) protocols. Azure file shares can be mounted concurrently by cloud or on-premises deployments. SMB Azure file shares are accessible from Windows, Linux, and macOS clients. NFS Azure file shares are accessible from Linux or macOS clients. Additionally, SMB Azure file shares can be cached on Windows Servers with Azure File Sync for fast access near where the data is being used.

Azure Files key benefits
- `Shared access`: SMB and NFS protocol support enables compatibility with existing applications.
- `Fully managed`: No server hardware or OS patching to manage.
- `Scripting and tooling`: Manage shares with Azure CLI, Azure PowerShell, the Azure portal, and Storage Explorer.
- `Resiliency`: Built for high availability.
- `Familiar programmability`: Applications can use standard file I/O APIs plus Azure SDKs and REST APIs.

`Azure Disk` storage (managed disks) provides block-level volumes for Azure VMs. They are virtualized and managed by Azure for improved resiliency and simpler operations.

`Azure Table` storage is a NoSQL store for large amounts of structured, non-relational data, accessible through authenticated calls from cloud and hybrid environments.

Azure Migrate is a service that helps you migrate from an on-premises environment to the cloud:
+ `Unified migration platform`: A single portal to start, run, and track your migration to Azure.
+ `Range of tools`: A range of tools for assessment and migration. Azure Migrate tools include `Discovery and assessment` and `Server Migration`. Azure Migrate also integrates with other Azure services and tools, and with *independent software vendor* (ISV) offerings.
+ `Assessment and migration`: In the Azure Migrate hub, you can assess and migrate your on-premises infrastructure to Azure.

`Azure Data Box` is a physical migration service that helps transfer large amounts of data in a quick, inexpensive, and reliable way. The secure data transfer is accelerated by shipping you a proprietary Data Box storage device that has a maximum usable storage capacity of 80 terabytes.
Data Box is ideally suited for moving very large data sets (often tens of terabytes or more) when network bandwidth is limited:
- One-time bulk migration of on-premises data to Azure.
- Periodic large data uploads where online transfer is too slow.
- Exporting large data sets from Azure for recovery or regulatory needs.

Once the data from your import order is uploaded to Azure, the disks on the device are wiped clean in accordance with NIST 800-88r1 standards. For an export order, the disks are erased once the device reaches the Azure datacenter.

In addition to large-scale migration using services like Azure Migrate and Azure Data Box, Azure also has tools designed to help you move or interact with individual files or small file groups. Among those tools are *AzCopy*, *Azure Storage Explorer*, and *Azure File Sync*.

*AzCopy* is a command-line utility that you can use to copy blobs or files to or from your storage account. With AzCopy, you can upload files, download files, copy files between storage accounts, and even synchronize files. AzCopy can even be configured to work with other cloud providers.
Synchronizing blobs or files with AzCopy is one-direction synchronization. When you synchronize, you designate the source and destination, and AzCopy will copy files or blobs in that direction. It doesn't synchronize bi-directionally based on timestamps or other metadata.

*Azure Storage Explorer* is a standalone app that provides a graphical interface to manage files and blobs in your Azure Storage Account. It works on Windows, macOS, and Linux operating systems and uses AzCopy on the backend to perform all of the file and blob management tasks. With Storage Explorer, you can upload to Azure, download from Azure, or move between storage accounts.

*Azure File Sync* is a tool that lets you centralize your file shares in Azure Files. Once you install Azure File Sync on your local Windows server, it will automatically stay bi-directionally synced with your files in Azure. With Azure File Sync, you can:
- Use any protocol that's available on Windows Server to access your data locally, including SMB, NFS, and FTPS.
- Have as many caches as you need across the world.
- Replace a failed local server by installing Azure File Sync on a new server in the same datacenter.
- Configure cloud tiering so the most frequently accessed files are replicated locally, while infrequently accessed files are kept in the cloud until requested.

