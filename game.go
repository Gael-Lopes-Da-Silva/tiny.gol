package main

import (
	"strconv"

	raylib "github.com/gen2brain/raylib-go/raylib"
)

const SCREEN_WIDTH = 1080
const SCREEN_HEIGHT = 720

var cellGrid [GRID_WIDTH][GRID_HEIGHT]cell
var mousePosition raylib.Vector2
var mouseX int
var mouseY int
var pause bool
var patterns bool
var counter int

func start() {
	raylib.InitWindow(SCREEN_WIDTH, SCREEN_HEIGHT, "GameOfLife")
	raylib.SetExitKey(0)
	raylib.SetTargetFPS(60)

	cellGrid = initCellGrid(cellGrid)
	pause = true
	patterns = false
}

func update(deltaTime float32) {
	// pause
	if raylib.IsKeyPressed(raylib.KeySpace) {
		pause = !pause
		if pause {
			raylib.SetTargetFPS(60)
		} else {
			raylib.SetTargetFPS(15)
		}
	}

	// mouse update
	mousePosition = raylib.GetMousePosition()
	mouseX = int(mousePosition.X / CELL_SIZE)
	mouseY = int(mousePosition.Y / CELL_SIZE)

	if pause && raylib.IsMouseButtonPressed(raylib.MouseLeftButton) {
		cellGrid[mouseX][mouseY].isAlive = !cellGrid[mouseX][mouseY].isAlive
	}

	// menu
	if pause && raylib.IsKeyPressed(raylib.KeyC) {
		cellGrid = clearCellGrid(cellGrid)
		counter = 0
	}

	if pause && raylib.IsKeyPressed(raylib.KeyR) {
		cellGrid = randomizeCellGrid(cellGrid)
		counter = 0
	}

	if pause && raylib.IsKeyPressed(raylib.KeyP) {
		patterns = !patterns
	}

	// patterns menu
	if pause && patterns && raylib.IsKeyPressed(raylib.KeyOne) {
		cellGrid = blockPattern(cellGrid)
		counter = 0
	}
	if pause && patterns && raylib.IsKeyPressed(raylib.KeyTwo) {
		cellGrid = blinkerPattern(cellGrid)
		counter = 0
	}
	if pause && patterns && raylib.IsKeyPressed(raylib.KeyThree) {
		cellGrid = gliderPattern(cellGrid)
		counter = 0
	}
	if pause && patterns && raylib.IsKeyPressed(raylib.KeyFour) {
		cellGrid = pentominoPattern(cellGrid)
		counter = 0
	}

	// cells update
	if !pause {
		cellGrid = updateCellGrid(cellGrid)
		counter++
	}
}

func process(deltaTime float32) {
	raylib.BeginDrawing()
	{
		raylib.ClearBackground(raylib.White)

		for x, cells := range cellGrid {
			for y, cell := range cells {
				cell.drawCell()

				if x == mouseX && y == mouseY && pause {
					raylib.DrawRectangleRounded(raylib.NewRectangle(cell.position.X, cell.position.Y, CELL_SIZE, CELL_SIZE), 0.24, 0, raylib.NewColor(13, 86, 202, 200))
				}
			}
		}

		if pause {
			raylib.DrawText("PAUSE", (SCREEN_WIDTH/2)-100, 20, 60, raylib.Red)
			raylib.DrawRectangleRounded(raylib.NewRectangle(5, 5, 160, 70), 0.2, 0, raylib.NewColor(36, 36, 36, 200))
			raylib.DrawText("(C) Clear", 10, 10, 20, raylib.Blue)
			raylib.DrawText("(R) Randomize", 10, 30, 20, raylib.Blue)
			raylib.DrawText("(P) Patterns", 10, 50, 20, raylib.Blue)

			raylib.DrawRectangleRounded(raylib.NewRectangle(5, 85, 160, 30), 0.2, 0, raylib.NewColor(36, 36, 36, 200))
			raylib.DrawText("Turn: "+strconv.Itoa(counter), 10, 90, 20, raylib.Blue)
		}

		if pause && patterns {
			raylib.DrawRectangleRounded(raylib.NewRectangle(170, 5, 160, 90), 0.2, 0, raylib.NewColor(36, 36, 36, 200))
			raylib.DrawText("(1) Block", 175, 10, 20, raylib.Blue)
			raylib.DrawText("(2) Blinker", 175, 30, 20, raylib.Blue)
			raylib.DrawText("(3) Glider", 175, 50, 20, raylib.Blue)
			raylib.DrawText("(4) Pentomino", 175, 70, 20, raylib.Blue)
		}
	}
	raylib.EndDrawing()
}

func processEnd() {
	raylib.CloseWindow()
}

func main() {
	start()
	for !raylib.WindowShouldClose() {
		deltaTime := raylib.GetFrameTime()
		update(deltaTime)
		process(deltaTime)
	}
	processEnd()
}
