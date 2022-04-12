dotnet publish -c Release -r win-x64 -p:PublishReadyToRun=true -p:PublishSingleFile=true --self-contained false
dotnet publish -c Release -r win-x86 -p:PublishReadyToRun=true -p:PublishSingleFile=true --self-contained false
dotnet publish -c Release -r linux-x64 -p:PublishReadyToRun=true -p:PublishSingleFile=true --self-contained false
dotnet publish -c Release -r osx-x64 -p:PublishReadyToRun=true -p:PublishSingleFile=true --self-contained false
