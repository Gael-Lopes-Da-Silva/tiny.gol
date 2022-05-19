if [ -d "./build" ]; then
    rm -rf "./build"
fi

mkdir "./build"
mkdir "./build/windows"
mkdir "./build/windows/windows-x64"
mkdir "./build/windows/windows-x86"
mkdir "./build/linux"
mkdir "./build/linux/linux-x64"
mkdir "./build/linux/linux-arm"
mkdir "./build/macos"

dotnet publish -c Release -o ./build/windows/windows-x64 -r win-x64 --self-contained true
dotnet publish -c Release -o ./build/windows/windows-x86 -r win-x86 --self-contained true
dotnet publish -c Release -o ./build/linux/linux-x64 -r linux-x64 --self-contained true
dotnet publish -c Release -o ./build/linux/linux-arm -r linux-arm --self-contained true
dotnet publish -c Release -o ./build/macos -r osx-x64 --self-contained true
