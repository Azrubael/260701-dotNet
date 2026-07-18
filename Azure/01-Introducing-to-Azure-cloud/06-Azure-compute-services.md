**QUIZ**
-----------
1. Which Azure Virtual Machine feature staggers updates across VMs based on their update domain and fault domain?
Availability sets

2. Which Azure compute option is event-driven and serverless, so you can run code without managing virtual machines?
Azure Functions

3. Which Azure service lets organizations host web apps and APIs without managing the underlying infrastructure?
Azure App Service

4. Which Azure service category provides prebuilt APIs for capabilities like vision, speech, and language without training your own model from scratch?
Azure AI services.


Virtual machine availability sets improve VM resiliency inside a region
Availability sets group VMs by:
- Update domain: VMs that can be rebooted together during planned maintenance.
- Fault domain: VMs that share a potential power or network failure point.

`Azure Virtual Desktop is` a desktop and application virtualization service in Azure. It lets users securely access Windows desktops and apps from many device types and locations.

`Containers` are a virtualization environment. Much like running multiple virtual machines on a single physical host, you can run multiple containers on a single physical or virtual host. Each virtual machine runs its own operating system that you can connect to and manage. Containers are lightweight and designed to be created, scaled out, and stopped dynamically.

`Azure Container Instances` are a platform as a service *PaaS* offering.
`Azure Container Apps` are similar in many ways to a container instance. They let you get up and running right away, they remove the container management overhead, and they're a *PaaS* offering. Container Apps also include built-in load balancing and scaling, so your design can adapt to changing demand.
`Azure Kubernetes Service` (AKS) is a container orchestration service. An orchestration service manages the lifecycle of containers. When you're deploying a fleet of containers, *AKS* can make fleet management simpler and more efficient.

Containers are often used to create solutions that use a `microservice architecture`. In this architecture, you break solutions into smaller, independent pieces. For example, you might split a website into a container hosting your front end, another hosting your back end, and a third for storage. This split lets you maintain, scale, or update each part of your app independently.

`Azure Functions` is an event-driven, serverless compute option that doesn’t require maintaining virtual machines or containers.
Using `Azure Functions` is ideal when you're only concerned about the code running your service and not about the underlying platform or infrastructure. Functions are commonly used when you need to perform work in response to an event (often via a REST request), timer, or message from another Azure service, and when that work can be completed quickly, within seconds or less.
Functions scale automatically based on demand, so they may be a good choice when demand is variable.

Functions can be either `stateless or stateful`.
When they're stateless (the default), they behave as if they restart every time they respond to an event. 
When they're stateful (called Durable Functions), the runtime passes a context through the function to track prior activity.

Use `Azure AI Services` if:
    You need ready-to-use AI APIs (e.g., extracting text from images, translating speech).
    You want low-code solutions (e.g., drag-and-drop AI in Power Platform).
    You need custom vision or document processing (e.g., receipt scanning).
Use `Azure OpenAI Services` if:
    You need generative AI (e.g., chatbots, content generation, code completion).
    You want to fine-tune OpenAI models for specialized tasks.
    You require advanced NLP 
	
Use `Azure Machine Learning` when you need to build, train, and manage custom machine learning models. This option is a better fit when your scenario requires model development, experimentation, and lifecycle management.

Azure IoT services help you connect, monitor, and manage devices.
+ `Azure IoT Hub` enables secure, bi-directional communication between cloud services and IoT devices.
+ `Azure IoT Central` provides a simplified software as a service (SaaS) IoT platform for solution builders.
+ `Azure IoT Edge` extends cloud capabilities to edge devices so some workloads can run closer to where data is generated.

Use Azure AI services when you need prebuilt AI features exposed through APIs.
Use Azure Machine Learning when you need custom model development and management.
Use Azure IoT services when your solution centers on connected devices and telemetry.

`Azure App Service` lets you build and host web apps, background jobs, mobile back-ends, and RESTful APIs in the programming language of your choice without managing infrastructure. It offers automatic scaling and high availability. App Service supports Windows and Linux and supports automated deployments from GitHub, Azure DevOps, or any Git repo for continuous deployment.
Types of app services:
		Web apps
		API apps
		WebJobs
		Mobile apps
`Azure App Service` includes full support for hosting web apps by using ASP.NET, ASP.NET Core, Java, Ruby, Node.js, PHP, or Python.

Much like hosting a website, you can build REST-based web APIs by using your choice of language and framework. App Service provides full Swagger support and the ability to package and publish your API in Azure Marketplace. The published APIs can be consumed from any HTTP- or HTTPS-based client.

You can use the WebJobs feature to run a program (.exe, Java, PHP, Python, or Node.js) or script (.cmd, .bat, PowerShell, or Bash) in the same context as a web app, API app, or mobile app. They can be scheduled or run by a trigger. WebJobs are often used to run background tasks as part of your application logic.

