/* 
@author: Gael Lopes Da Silva
@project: Brainfuck Interpreter
@github: https://github.com/Gael-Lopes-Da-Silva/Brainfuck
*/

using System.Runtime.InteropServices;

string target = string.Empty;

void WriteDebugMessage(string message)
{
    ForegroundColor = ConsoleColor.Green;
    Write("+ ");
    ResetColor();
    WriteLine(message);
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
            target = "-r win-x64";
        }
        else
        {
            target = "-r win-x86";
        }
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        target = "-r linux-x64";

        if (RuntimeInformation.ProcessArchitecture == Architecture.Arm)
        {
            target = "-r linux-arm";
        }
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
        target = "-r osx-x64";
    }

    build.StartInfo.Arguments = $"publish -c Release -o ./build {target} --self-contained true";

    build.Start();
    build.WaitForExit();
    build.Close();

    WriteDebugMessage("Build finished");
}

BuildProject();
