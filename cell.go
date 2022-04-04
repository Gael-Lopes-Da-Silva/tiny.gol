package main

import (
	"math/rand"
	"time"

	raylib "github.com/gen2brain/raylib-go/raylib"
)

const CELL_SIZE = 24
const GRID_WIDTH = SCREEN_WIDTH / CELL_SIZE
const GRID_HEIGHT = SCREEN_HEIGHT / CELL_SIZE

type cell struct {
	isAlive  bool
	color    raylib.Color
	position raylib.Vector2
	size     raylib.Vector2
}

func (c cell) drawCell() {
	if c.isAlive {
		raylib.DrawRectangleV(c.position, c.size, c.color)
	}
}

func newCell(x int, y int, size int, color raylib.Color) cell {
	var newCell cell
	newCell.isAlive = false
	newCell.color = color
	newCell.position = raylib.NewVector2(float32(x*size), float32(y*size))
	newCell.size = raylib.NewVector2(float32(size), float32(size))
	return newCell
}

func initCellGrid(cellGrid [GRID_WIDTH][GRID_HEIGHT]cell) [GRID_WIDTH][GRID_HEIGHT]cell {
	newCellGrid := cellGrid

	for x := 0; x < GRID_WIDTH; x++ {
		for y := 0; y < GRID_HEIGHT; y++ {
			newCellGrid[x][y] = newCell(x, y, CELL_SIZE, raylib.Black)
		}
	}

	newCellGrid = randomizeCellGrid(newCellGrid)

	return newCellGrid
}

func clearCellGrid(cellGrid [GRID_WIDTH][GRID_HEIGHT]cell) [GRID_WIDTH][GRID_HEIGHT]cell {
	newCellGrid := cellGrid

	for x := 0; x < GRID_WIDTH; x++ {
		for y := 0; y < GRID_HEIGHT; y++ {
			newCellGrid[x][y].isAlive = false
		}
	}

	return newCellGrid
}

func randomizeCellGrid(cellGrid [GRID_WIDTH][GRID_HEIGHT]cell) [GRID_WIDTH][GRID_HEIGHT]cell {
	rand.Seed(time.Now().UnixNano())
	newCellGrid := cellGrid

	for x := 0; x < GRID_WIDTH; x++ {
		for y := 0; y < GRID_HEIGHT; y++ {
			randomBool := rand.Intn(2) == 1

			newCellGrid[x][y].isAlive = randomBool
		}
	}

	return newCellGrid
}

func updateCellGrid(cellGrid [GRID_WIDTH][GRID_HEIGHT]cell) [GRID_WIDTH][GRID_HEIGHT]cell {
	newCellGrid := cellGrid

	for x := 0; x < GRID_WIDTH; x++ {
		for y := 0; y < GRID_HEIGHT; y++ {
			neighbours := countCellNeigbours(x, y, cellGrid)

			if !cellGrid[x][y].isAlive && neighbours == 3 {
				newCellGrid[x][y].isAlive = true
			} else if cellGrid[x][y].isAlive && (neighbours < 2 || neighbours > 3) {
				newCellGrid[x][y].isAlive = false
			}
		}
	}

	return newCellGrid
}

func countCellNeigbours(x int, y int, cellGrid [GRID_WIDTH][GRID_HEIGHT]cell) int {
	neighbours := 0

	// top row
	if x-1 > 0 {
		if cellGrid[x-1][y].isAlive {
			neighbours++
		}

		if y-1 > 0 && cellGrid[x-1][y-1].isAlive {
			neighbours++
		}

		if y+1 < GRID_HEIGHT && cellGrid[x-1][y+1].isAlive {
			neighbours++
		}
	}

	// midlle row
	if y-1 > 0 && cellGrid[x][y-1].isAlive {
		neighbours++
	}

	if y+1 < GRID_HEIGHT && cellGrid[x][y+1].isAlive {
		neighbours++
	}

	// bottom row
	if x+1 < GRID_WIDTH {
		if cellGrid[x+1][y].isAlive {
			neighbours++
		}

		if y+1 < GRID_HEIGHT && cellGrid[x+1][y+1].isAlive {
			neighbours++
		}

		if y-1 > 0 && cellGrid[x+1][y-1].isAlive {
			neighbours++
		}
	}

	return neighbours
}

func blockPattern(cellGrid [GRID_WIDTH][GRID_HEIGHT]cell) [GRID_WIDTH][GRID_HEIGHT]cell {
	newCellGrid := clearCellGrid(cellGrid)

	newCellGrid[25][17].isAlive = true
	newCellGrid[25][18].isAlive = true
	newCellGrid[26][17].isAlive = true
	newCellGrid[26][18].isAlive = true

	return newCellGrid
}

func blinkerPattern(cellGrid [GRID_WIDTH][GRID_HEIGHT]cell) [GRID_WIDTH][GRID_HEIGHT]cell {
	newCellGrid := clearCellGrid(cellGrid)

	newCellGrid[25][17].isAlive = true
	newCellGrid[25][18].isAlive = true
	newCellGrid[25][19].isAlive = true

	return newCellGrid
}

func gliderPattern(cellGrid [GRID_WIDTH][GRID_HEIGHT]cell) [GRID_WIDTH][GRID_HEIGHT]cell {
	newCellGrid := clearCellGrid(cellGrid)

	newCellGrid[25][19].isAlive = true
	newCellGrid[26][19].isAlive = true
	newCellGrid[27][19].isAlive = true
	newCellGrid[27][18].isAlive = true
	newCellGrid[26][17].isAlive = true

	return newCellGrid
}

func pentominoPattern(cellGrid [GRID_WIDTH][GRID_HEIGHT]cell) [GRID_WIDTH][GRID_HEIGHT]cell {
	newCellGrid := clearCellGrid(cellGrid)

	newCellGrid[26][19].isAlive = true
	newCellGrid[27][17].isAlive = true
	newCellGrid[26][18].isAlive = true
	newCellGrid[25][18].isAlive = true
	newCellGrid[26][17].isAlive = true

	return newCellGrid
}
