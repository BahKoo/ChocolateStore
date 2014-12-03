ChocolateStore
==============
Cache chocolatey packages to efficiently provision multiple machines or VMs on a LAN

### LICENSE
Apache 2.0 - see LICENSE

### COMPILATION REQUIREMENTS
* Visual Studio 2010
* .NET Framewrok 4.0
* NuGet Package Manager with "Allow NuGet to download missing packages" setting enabled

### SYNTAX
`ChocolateStore \<directory\> \<url\>`

### EXAMPLE
In this example, we will store the latest version of GoogleChrome on a network share and install it from a client on the LAN.

1) In a command prompt, browse to the ChocolateStore "bin" folder.

2) Execute the following command. Note that the first argument is a network share for which the current user has "write" permissions. This will download the GoogleChrome package, download the chrome installer and modifer the package to point to the local installer.

`ChocolateStore M:\Store http://chocolatey.org/api/v2/package/GoogleChrome/`

3) From a computer that would like to have GoogleChrome installed and from which the current user has "read" permissions to the network share execute the following command:

`cinst GoogleChrome -source M:\Store`
