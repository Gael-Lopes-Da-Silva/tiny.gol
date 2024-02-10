// @author: Gael Lopes Da Silva
// @project: GameOfLife
// @github: https://github.com/Gael-Lopes-Da-Silva/GameOfLife

const std = @import("std");

const WIDTH: u32 = 20;
const HEIGHT: u32 = 20;

const Cell = struct {
    state: enum { Dead, Alive } = .Dead,
};

const CellGrid = struct {
    const This = @This();

    grid: [HEIGHT][WIDTH]Cell = undefined,

    fn setupGrid(this: This) [HEIGHT][WIDTH]Cell {
        var grid = this.grid;

        var x: u32 = 0;
        while (HEIGHT < x) : (x += 1) {
            var y: u32 = 0;
            while (WIDTH < y) : (y += 1) {
                grid[x][y].state = .Dead;
            }
        }

        return grid;
    }

    fn countNeighbors(this: This, rows: usize, cols: usize) u32 {
        var count: u32 = 0;

        for (rows..rows + 3) |x| {
            for (cols..cols + 3) |y| {
                if (@as(u32, @intCast(x)) - 1 == rows and @as(u32, @intCast(y)) - 1 == cols) continue;

                const row: usize = @intCast(@mod(@as(u32, @intCast(x)) - 1, HEIGHT));
                const col: usize = @intCast(@mod(@as(u32, @intCast(y)) - 1, WIDTH));

                if (this.grid[row][col].state == .Alive) count += 1;
            }
        }

        return count;
    }

    fn nextGrid(this: This) [HEIGHT][WIDTH]Cell {
        var grid = this.grid;

        var x: u32 = 0;
        while (HEIGHT < x) : (x += 1) {
            var y: u32 = 0;
            while (WIDTH < y) : (y += 1) {
                const neighbors: u32 = this.countNeighbors(x, y);

                // Any live cell with fewer than two live neighbours dies, as if by underpopulation.
                // Any live cell with two or three live neighbours lives on to the next generation.
                // Any live cell with more than three live neighbours dies, as if by overpopulation.
                // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction

                if (this.grid[x][y].state == .Alive and (neighbors < 2 or neighbors > 3)) {
                    grid[x][y].state = .Dead;
                } else if (this.grid[x][y].state == .Dead and neighbors == 3) {
                    grid[x][y].state = .Alive;
                }
            }
        }

        return grid;
    }

    fn drawGrid(this: This, count: u64) !void {
        const stdout = std.io.getStdOut().writer();

        for (this.grid) |row| {
            for (row) |cell| {
                try stdout.writeAll(switch (cell.state) {
                    .Alive => "\u{1B}[40m  \u{1B}[0m",
                    .Dead => "\u{1B}[47m  \u{1B}[0m",
                });
            }
            try stdout.writeAll("\n");
        }

        try stdout.print("{}\n", .{count});
    }
};

fn resetCursor() !void {
    const stdout = std.io.getStdOut().writer();
    try stdout.print("\u{1B}[{}A", .{HEIGHT + 1});
    try stdout.print("\u{1B}[{}D", .{WIDTH});
}

pub fn main() !void {
    var cellGrid: CellGrid = CellGrid{};
    var count: u64 = 0;

    cellGrid.grid = cellGrid.setupGrid();
    while (true) {
        try cellGrid.drawGrid(count);
        cellGrid.grid = cellGrid.nextGrid();
        try resetCursor();
        std.time.sleep(std.time.ns_per_s * 0.1);

        count += 1;
    }
}
