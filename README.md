<h3 align="center">Game of Life</h3>

---

<p align="center">⚙️ This is my own implementation of the <a href="https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life">Conway's Game of Life</a> in C# with <a href="https://www.raylib.com/">Raylib</a>.</p>

---
### 🖼️ Screenshots
![screenshot1](./Screenshots/s1.png)
![screenshot2](./Screenshots/s2.png)
![screenshot3](./Screenshots/s3.png)

### ❓ How to use

- C - Clear all cells
- R - Randomize all cells
- P - Open the patterns menu
  - 1 - Create a block
  - 2 - Create a blinker
  - 3 - Create a glider
  - 4 - Create a pentomino
- H - Hide the GUI
- ESC - Close windows

### ❓ How to build

First install the latest dotnet SDK <a href="https://dotnet.microsoft.com/en-us/download">here</a>.

Then run these commands:
~~~shell
$ dotnet tool install -g dotnet-script
$ dotnet script ./Source/build.csx
~~~
