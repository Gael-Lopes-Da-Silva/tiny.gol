/* 
@author: Gael Lopes Da Silva
@project: Conway's game of life
@github: https://github.com/Gael-Lopes-Da-Silva/Brainfuck
@gitlab: https://gitlab.com/Gael-Lopes-Da-Silva/Brainfuck
*/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.MouseButton;

class GameOfLife
{
    private const int SCREEN_WIDTH = 1080;
    private const int SCREEN_HEIGHT = 720;
    private const int CELL_SIZE = 24;
    private const int GRID_WIDTH = SCREEN_WIDTH / CELL_SIZE;
    private const int GRID_HEIGHT = SCREEN_HEIGHT / CELL_SIZE;

    private static Cell[,] cellGrid = new Cell[GRID_HEIGHT, GRID_WIDTH];
    private static Camera2D camera = new Camera2D();
    private static Vector2 mousePosition;
    private static int mouseX;
    private static int mouseY;
    private static int loopCounter;
    private static bool pauseMenu;
    private static bool patternMenu;
    private static bool hideGUI;

    public static void Main()
    {
        // start
        InitWindow(SCREEN_WIDTH, SCREEN_HEIGHT, "Game Of Life");
        SetWindowIcon(LoadImage("./icon.png"));
        SetTargetFPS(60);

        cellGrid = Cell.InitCellGrid(cellGrid);
        cellGrid = Cell.RandomizeCellGrid(cellGrid);
        pauseMenu = true;
        patternMenu = false;
        hideGUI = false;

        camera.zoom = 1f;
        camera.target = new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2);

        while (!WindowShouldClose())
        {
            // update
            mousePosition = GetMousePosition();
            mouseX = ((int)mousePosition.X) / CELL_SIZE;
            mouseY = ((int)mousePosition.Y) / CELL_SIZE;

            camera.offset = new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2);

            if (IsKeyPressed(KEY_SPACE))
            {
                pauseMenu = !pauseMenu;

                if (pauseMenu)
                {
                    SetTargetFPS(60);
                }
                else
                {
                    SetTargetFPS(15);
                }

                if (hideGUI)
                {
                    hideGUI = false;
                }
            }

            if (pauseMenu)
            {
                if (IsMouseButtonPressed(MOUSE_LEFT_BUTTON))
                {
                    cellGrid[mouseY, mouseX].alive = !cellGrid[mouseY, mouseX].alive;
                }

                if (IsKeyPressed(KEY_C))
                {
                    cellGrid = Cell.ClearCellGrid(cellGrid);
                    loopCounter = 0;
                }

                if (IsKeyPressed(KEY_R))
                {
                    cellGrid = Cell.RandomizeCellGrid(cellGrid);
                    loopCounter = 0;
                }

                if (IsKeyPressed(KEY_P))
                {
                    patternMenu = !patternMenu;
                }

                if (IsKeyPressed(KEY_H))
                {
                    hideGUI = !hideGUI;
                }

                if (patternMenu)
                {
                    if (IsKeyPressed(KEY_ONE))
                    {
                        loopCounter = 0;
                        cellGrid = Cell.ClearCellGrid(cellGrid);
                        cellGrid = Cell.BlockPattern(cellGrid);
                    }
                    if (IsKeyPressed(KEY_TWO))
                    {
                        loopCounter = 0;
                        cellGrid = Cell.ClearCellGrid(cellGrid);
                        cellGrid = Cell.BlinkerPattern(cellGrid);
                    }
                    if (IsKeyPressed(KEY_THREE))
                    {
                        loopCounter = 0;
                        cellGrid = Cell.ClearCellGrid(cellGrid);
                        cellGrid = Cell.GliderPattern(cellGrid);
                    }
                    if (IsKeyPressed(KEY_FOUR))
                    {
                        loopCounter = 0;
                        cellGrid = Cell.ClearCellGrid(cellGrid);
                        cellGrid = Cell.PentominoPattern(cellGrid);
                    }
                }
            }

            if (!pauseMenu)
            {
                cellGrid = Cell.UpdateCellGrid(cellGrid);
                loopCounter++;
            }

            // process
            BeginDrawing();
            ClearBackground(RED);
            BeginMode2D(camera);
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    cellGrid[y, x].DrawCell();

                    if (x == mouseX && y == mouseY && pauseMenu)
                    {
                        DrawRectangle(((int)cellGrid[y, x].position.X), ((int)cellGrid[y, x].position.Y), CELL_SIZE, CELL_SIZE, new Color(13, 86, 202, 200));
                    }
                }
            }
            EndMode2D();

            DrawText($"Turn: {loopCounter}", 10, GetScreenHeight() - 30, 20, BLUE);

            if (pauseMenu)
            {
                DrawText("PAUSE", (GetScreenWidth() / 2) - 100, 20, 60, RED);

                if (!hideGUI)
                {
                    DrawRectangleRounded(new Rectangle(5, 5, 160, 90), 0.2f, 0, new Color(36, 36, 36, 255));
                    DrawText("(C) Clear", 10, 10, 20, BLUE);
                    DrawText("(R) Randomize", 10, 30, 20, BLUE);
                    DrawText("(P) Patterns", 10, 50, 20, BLUE);
                    DrawText("(H) Hide GUI", 10, 70, 20, BLUE);

                    DrawRectangleRounded(new Rectangle(5, GetScreenHeight() - 35, 160, 30), 0.2f, 0, new Color(36, 36, 36, 255));
                    DrawText($"Turn: {loopCounter}", 10, GetScreenHeight() - 30, 20, BLUE);
                }

                if (patternMenu)
                {
                    DrawRectangleRounded(new Rectangle(170, 5, 160, 90), 0.2f, 0, new Color(36, 36, 36, 255));
                    DrawText("(1) Block", 175, 10, 20, BLUE);
                    DrawText("(2) Blinker", 175, 30, 20, BLUE);
                    DrawText("(3) Glider", 175, 50, 20, BLUE);
                    DrawText("(4) Pentomino", 175, 70, 20, BLUE);
                }
            }
            EndDrawing();
        }
        // end process
        CloseWindow();
    }

    class Cell
    {
        public bool alive;
        public Color color;
        public Vector2 position;
        public Vector2 size;

        public Cell(int x, int y, int size, Color color)
        {
            this.alive = false;
            this.color = color;
            this.position = new Vector2(x * size, y * size);
            this.size = new Vector2(size, size);
        }

        public void DrawCell()
        {
            if (this.alive)
            {
                DrawRectangleV(this.position, this.size, this.color);
            }
            else
            {
                DrawRectangleV(this.position, this.size, WHITE);
            }
        }

        public static Cell[,] InitCellGrid(Cell[,] cellGrid)
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    cellGrid[y, x] = new Cell(x, y, CELL_SIZE, BLACK);
                }
            }

            return cellGrid;
        }

        public static Cell[,] ClearCellGrid(Cell[,] cellGrid)
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    cellGrid[y, x].alive = false;
                }
            }

            return cellGrid;
        }

        public static Cell[,] RandomizeCellGrid(Cell[,] cellGrid)
        {
            var randomBoolean = new Random();

            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    cellGrid[y, x].alive = randomBoolean.Next(2) == 1;
                }
            }

            return cellGrid;
        }

        public static Cell[,] UpdateCellGrid(Cell[,] cellGrid)
        {
            var newCellGrid = CloneCellGrid(cellGrid);

            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    int neighbours = CountCellNeighbours(x, y, cellGrid);

                    if (!cellGrid[y, x].alive && neighbours == 3)
                    {
                        newCellGrid[y, x].alive = true;
                    }
                    else if (cellGrid[y, x].alive && (neighbours < 2 || neighbours > 3))
                    {
                        newCellGrid[y, x].alive = false;
                    }
                }
            }

            return newCellGrid;
        }

        public static Cell[,] CloneCellGrid(Cell[,] cellGrid)
        {
            var newCellGrid = new Cell[GRID_HEIGHT, GRID_WIDTH];

            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    newCellGrid[y, x] = new Cell(x, y, CELL_SIZE, BLACK);
                    newCellGrid[y, x].alive = cellGrid[y, x].alive;
                }
            }

            return newCellGrid;
        }

        public static int CountCellNeighbours(int x, int y, Cell[,] cellGrid)
        {
            int neighbours = 0;

            if (x - 1 > 0)
            {
                if (cellGrid[y, x - 1].alive) neighbours++;
                if (y - 1 > 0 && cellGrid[y - 1, x - 1].alive) neighbours++;
                if (y + 1 < GRID_HEIGHT - 1 && cellGrid[y + 1, x - 1].alive) neighbours++;
            }

            if (y - 1 > 0 && cellGrid[y - 1, x].alive) neighbours++;
            if (y + 1 < GRID_HEIGHT - 1 && cellGrid[y + 1, x].alive) neighbours++;

            if (x + 1 < GRID_WIDTH - 1)
            {
                if (cellGrid[y, x + 1].alive) neighbours++;
                if (y + 1 < GRID_HEIGHT - 1 && cellGrid[y + 1, x + 1].alive) neighbours++;
                if (y - 1 > 0 && cellGrid[y - 1, x + 1].alive) neighbours++;
            }

            return neighbours;
        }

        public static Cell[,] BlockPattern(Cell[,] cellGrid)
        {
            cellGrid[15, 22].alive = true;
            cellGrid[16, 22].alive = true;
            cellGrid[15, 23].alive = true;
            cellGrid[16, 23].alive = true;

            return cellGrid;
        }

        public static Cell[,] BlinkerPattern(Cell[,] cellGrid)
        {
            cellGrid[15, 23].alive = true;
            cellGrid[16, 23].alive = true;
            cellGrid[17, 23].alive = true;

            return cellGrid;
        }

        public static Cell[,] GliderPattern(Cell[,] cellGrid)
        {
            cellGrid[17, 22].alive = true;
            cellGrid[17, 23].alive = true;
            cellGrid[17, 24].alive = true;
            cellGrid[16, 24].alive = true;
            cellGrid[15, 23].alive = true;

            return cellGrid;
        }

        public static Cell[,] PentominoPattern(Cell[,] cellGrid)
        {
            cellGrid[17, 23].alive = true;
            cellGrid[15, 24].alive = true;
            cellGrid[16, 23].alive = true;
            cellGrid[16, 22].alive = true;
            cellGrid[15, 23].alive = true;

            return cellGrid;
        }
    }
}
