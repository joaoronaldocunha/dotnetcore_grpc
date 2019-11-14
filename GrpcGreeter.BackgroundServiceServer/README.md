# Steps to Create Windows Service

## Publish dotnet core

```
dotnet publish -c Release -o [path_to_publish]
```

## Create Windows Service

1. Run Command Prompt as Administrator

2. Remove any service "GrpcGreeter"

```
sc delete "GrpcGreeter"
```

3. Create service "GrpcGreeter"

```
sc create "GrpcGreeter" binPath=[path_to_publish]
```

4. Start service "GrpcGreeter"

```
sc start "GrpcGreeter" binPath=[path_to_publish]
```

5. Check "GrpcGreeter" service status

sc query "GrpcGreeter"

6. If needed, stop service "GrpcGreeter"

```
sc stop "GrpcGreeter"
```

## Check Log

1. Open "Event Viewer"

2. Under "Application and Services Logs", check the log for service "GrpcGreeter"

# References:

- [Creating a Windows Service with .NET Core 3.0](https://csharp.christiannagel.com/2019/10/15/windowsservice/)
- [Troubleshoot gRPC on .NET Core](https://docs.microsoft.com/en-us/aspnet/core/grpc/troubleshoot?view=aspnetcore-3.0)

