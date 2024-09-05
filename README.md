<div align="center">
	<h1>Tiny Gol</h1>
    <a href="https://github.com/Gael-Lopes-Da-Silva/tiny.gol">https://github.com/Gael-Lopes-Da-Silva/tiny.gol</a>
</div>


Description
------------------------------------------------------------------

This is my implementation of [Conway's Game of Life](https://en.wikipedia.org/wiki/Conway's_Game_of_Life) made in Rust with [Ruscii](https://github.com/lemunozm/ruscii).


Usage
------------------------------------------------------------------

Run the executable in a terminal and enjoy.
~~~
Keybindings:
    Down, Up, Left, Right    Move the cursor
    H, J, K, L               Move the cursor
    ], [                     Increase or decrease speed
    E                        Switch cell state at cursor
    N                        Go to next generation
    R                        Randomize the grid
    C                        Clear the grid
    F3                       Show debug infos
    Space                    Toggle pause
    Esc, Q                   Quit
~~~


Build From Source
------------------------------------------------------------------

Make sure to have a ready to use installation of rust. More info [here](https://www.rust-lang.org/tools/install).

~~~
git clone https://github.com/Gael-Lopes-Da-Silva/tiny.gol.git
cd tiny.gol
cargo build --release
~~~
