
## C# on Linux - demo5

This repository contains .NET Core demo that I use during some of my talks. Please note that this code contains hacks and should not be taken seriously - it serves only as demonstration of interacting with the Linux system.  
This demo is built on top of [Tom's DBus library](https://github.com/tmds/Tmds.DBus).

#### Resources:

* [redhatloves.net](http://fedoraloves.net)
* [fedoraloves.net](http://fedoraloves.net)
* [Presentation slides](http://redhat.slides.com/rjanekov/netcore-5?token=VGY5n8wo)
* [recording](https://www.youtube.com/watch?v=lY6stR3TKxo)

#### Steps:

1. For this demo we need dotnet sdk installed, and an IDE of your choice. You can find out more at [fedoraloves.net](http://fedoraloves.net)
1. Add NuGet repository for tmds.DBus
    1. Edit the NuGet configuration:  
    `$ vim ~/.nuget/NuGet/NuGet.Config`
    1. Add a new packageSource:  
    `<add key="tmds" value="https://www.myget.org/F/tmds/api/v3/index.json" protocolVersion="3" />`
1. Create a new console application:  
    `$ mkdir demo5 && cd demo5`  
    `$ dotnet new console`  
1. Edit the project file to include `tmds.DBus`  
    [`$ vim demo5.csproj`](https://github.com/RheaAyase/dotnettalks.demo5/blob/master/demo5.csproj)
1. Run `$ dotnet restore` to get the NuGet packages we need.
1. List DBus services and find objects for org.freedesktop.UPower:  
    `$ dotnet dbus list --bus system services | grep -i power`  
    -`org.freedesktop.UPower`  
    `$ dotnet dbus list --bus system --service org.freedesktop.UPower objects | head -1`  
    -`/org/freedesktop/UPower : org.freedesktop.UPower`
1. Generate C# interfaces for the UPower service:  
    `$ dotnet dbus codegen --bus system --service org.freedesktop.UPower`  
    -`Generated: /home/rjanek/dev/dotnettalks/demo5/UPower.DBus.cs`
1. [Play with it](https://github.com/RheaAyase/dotnettalks.demo5/blob/master/Program.cs) =)
    
