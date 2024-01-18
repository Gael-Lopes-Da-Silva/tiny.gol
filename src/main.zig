// @author: Gael Lopes Da Silva
// @project: GameOfLife
// @github: https://github.com/Gael-Lopes-Da-Silva/GameOfLife

const std = @import("std");

const WIDTH: i32 = 20;
const HEIGHT: i32 = 20;

const Cell = struct {
    state: enum { Dead, Alive } = .Dead,
};

fn countNeighbors(frame: [HEIGHT][WIDTH]Cell, rows: usize, cols: usize) i32 {
    var count: i32 = 0;

    for (rows..rows + 3) |x| {
        for (cols..cols + 3) |y| {
            if (@as(i32, @intCast(x)) - 1 == rows and @as(i32, @intCast(y)) - 1 == cols) continue;

            const row: usize = @intCast(@mod(@as(i32, @intCast(x)) - 1, HEIGHT));
            const col: usize = @intCast(@mod(@as(i32, @intCast(y)) - 1, WIDTH));

            if (frame[row][col].state == .Alive) count += 1;
        }
    }

    return count;
}

fn nextFrame(frame: [HEIGHT][WIDTH]Cell) [HEIGHT][WIDTH]Cell {
    var next: [HEIGHT][WIDTH]Cell = frame;

    for (frame, 0..) |row, x| {
        for (row, 0..) |cell, y| {
            const neighbors: i32 = countNeighbors(frame, x, y);

            // Any live cell with fewer than two live neighbours dies, as if by underpopulation.
            // Any live cell with two or three live neighbours lives on to the next generation.
            // Any live cell with more than three live neighbours dies, as if by overpopulation.
            // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction

            if (cell.state == .Alive and (neighbors < 2 or neighbors > 3)) {
                next[x][y].state = .Dead;
            } else if (cell.state == .Dead and neighbors == 3) {
                next[x][y].state = .Alive;
            }
        }
    }

    return next;
}

fn drawFrame(frame: [HEIGHT][WIDTH]Cell) !void {
    const stdout = std.io.getStdOut().writer();

    for (frame) |row| {
        for (row) |cell| {
            try stdout.writeAll(switch (cell.state) {
                .Alive => "\u{1B}[40m  \u{1B}[0m",
                .Dead => "\u{1B}[47m  \u{1B}[0m",
            });
        }
        try stdout.writeAll("\n");
    }
}

fn resetCursor() !void {
    const stdout = std.io.getStdOut().writer();
    try stdout.print("\u{1B}[{}A", .{HEIGHT});
    try stdout.print("\u{1B}[{}D", .{WIDTH});
}

pub fn main() !void {
    var frame: [HEIGHT][WIDTH]Cell = undefined;

    frame[0][1].state = .Alive;
    frame[1][2].state = .Alive;
    frame[2][2].state = .Alive;
    frame[2][1].state = .Alive;
    frame[2][0].state = .Alive;

    while (true) {
        try drawFrame(frame);
        frame = nextFrame(frame);
        try resetCursor();
        std.time.sleep(std.time.ns_per_s * 0.1);
    }
}
