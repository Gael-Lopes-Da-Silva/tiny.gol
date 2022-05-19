/* 
@author: Gael Lopes Da Silva
@project: Conway's game of life
@github: https://github.com/Gael-Lopes-Da-Silva/Brainfuck
*/

using System.Runtime.InteropServices;

void WriteDebugMessage(string message)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("+ ");
    Console.ResetColor();
    Console.WriteLine(message);
}

void ResetBuildFolder()
{
    if (Directory.Exists("./build")) 
    {
        Directory.Delete("./build", true);
        Directory.CreateDirectory("./build");

        WriteDebugMessage("Build directory cleared");
    }
}

void BuildProject()
{
    var build = new Process();
    build.StartInfo.FileName = "dotnet";
    build.StartInfo.CreateNoWindow = true;
    build.StartInfo.UseShellExecute = false;

    WriteDebugMessage("Build started...");
    
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        if (Environment.Is64BitOperatingSystem)
        {
            build.StartInfo.Arguments = "publish -c Release -o ./build -r win-x64 --self-contained true";
        }
        else
        {
            build.StartInfo.Arguments = "publish -c Release -o ./build -r win-x86 --self-contained true";
        }
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        build.StartInfo.Arguments = "publish -c Release -o ./build -r linux-x86 --self-contained true";

        if (RuntimeInformation.ProcessArchitecture == Architecture.Arm)
        {
            build.StartInfo.Arguments = "publish -c Release -o ./build -r linux-arm --self-contained true";
        }
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
        build.StartInfo.Arguments = "publish -c Release -o ./build -r osx-x64 --self-contained true";
    }

    build.Start();
    build.WaitForExit();

    WriteDebugMessage("Build finished");
}

ResetBuildFolder();
BuildProject();
