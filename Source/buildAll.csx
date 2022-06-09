/* 
@author: Gael Lopes Da Silva
@project: Brainfuck Interpreter
@github: https://github.com/Gael-Lopes-Da-Silva/Brainfuck
*/

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

    build.StartInfo.Arguments = "publish -c Release -o ../Build/Win-x64 -r win-x64 --self-contained true";

    build.Start();
    build.WaitForExit();
    WriteDebugMessage("Win-x64 builded");

    build.StartInfo.Arguments = "publish -c Release -o ../Build/Win-x86 -r win-x86 --self-contained true";

    build.Start();
    build.WaitForExit();
    WriteDebugMessage("Win-x86 builded");

    build.StartInfo.Arguments = "publish -c Release -o ../Build/Linux-x64 -r linux-x64 --self-contained true";

    build.Start();
    build.WaitForExit();
    WriteDebugMessage("Linux-x64 builded");

    build.StartInfo.Arguments = "publish -c Release -o ../Build/Linux-arm -r linux-arm --self-contained true";

    build.Start();
    build.WaitForExit();
    WriteDebugMessage("Linux-arm builded");

    build.StartInfo.Arguments = "publish -c Release -o ../Build/Osx-x64 -r osx-x64 --self-contained true";

    build.Start();
    build.WaitForExit();
    WriteDebugMessage("Osx-x64 builded");

    build.Close();
    WriteDebugMessage("Build finished");
}

BuildProject();
