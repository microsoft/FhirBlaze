# FHIRBlaze
Welcome to FHIRBlaze!

## Overview
FHIRBlaze is a minimally viable Blazor WebAssembly application demonstrating how to interact with HL7 data from a Fast Health Interoperability Resources (FHIR) API.

## Architecture
A Blazor WebAssembly (wasm) application using the pattern of micro-frontends implemented by lazy-loading UI "modules" only when requested by navigation events from the client.

## Get Started
Here's what you'll need to run this application in your own environment.

### FHIR API (R4)
You will need access to a FHIR API that meets the HL7 R4 spec requirements. 
Host your own with one of these popular offerings:
* [Azure API for FHIR](https://docs.microsoft.com/en-us/azure/healthcare-apis/fhir/fhir-portal-quickstart)
* [Azure FHIR Server (OSS)](https://github.com/microsoft/fhir-server/blob/main/docs/QuickstartDeployPortal.md)
* [Firely Server](https://fire.ly/products/firely-server/)

OR, use one of these [publicly available test servers](https://wiki.hl7.org/index.php?title=Publicly_Available_FHIR_Servers_for_testing).

### Web Host
There are many options for hosting a Blazor WebAssembly application. This repo contains a Github Action workflow to automatically deploy the site to Azure Static Web Apps. 
Follow [this tutorial](https://docs.microsoft.com/en-us/azure/static-web-apps/deploy-blazor) to learn more.

Other options include:
* Azure App Service
* Azure Blob Storage static web
* [and more...](https://docs.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/webassembly?view=aspnetcore-5.0)

### Next Steps
[Fork this repo](https://github.com/microsoft/fhirblaze/fork) to modify or add your own functionality.

## Contributing

This project welcomes contributions and suggestions. Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Community Discord
If you are interested in projects like this or connecting with the health and life sciences developer community, please join our Discord server at https://aka.ms/HLS-Discord. We're a technology agnostic community seeking to share and collaborate on all things related to developing healthcare solutions. For in-depth questions specific to this project, please use the "Discussions" tab on GitHub. We welcome your thoughts and feedback.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft 
trademarks or logos is subject to and must follow 
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
