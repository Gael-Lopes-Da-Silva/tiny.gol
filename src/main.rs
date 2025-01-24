// use std::io::Cursor;

use ruscii::app::*;
use ruscii::drawing::*;
use ruscii::gui::*;
use ruscii::keyboard::*;
use ruscii::spatial::*;
use ruscii::terminal::*;

use rand::prelude::*;

use rodio::*;

#[derive(Debug)]
enum Scenes {
    Start,
    Playing,
    Paused,
}

#[derive(Clone)]
struct Cell {
    alive: bool,
}

struct Pointer {
    position: Vec2,
    velocity: Vec2,
}

struct Game {
    dimension: Vec2,
    // stream_handle: OutputStreamHandle,
    scene: Scenes,
    pointer: Pointer,
    generation: u64,
    speed: u32,
    cell_grid: Vec<Vec<Cell>>,
}

impl Game {
    fn new(dimension: Vec2, /*stream_handle: OutputStreamHandle*/) -> Self {
        Self {
            dimension,
            // stream_handle,
            scene: Scenes::Start,
            pointer: Pointer {
                position: Vec2::zero(),
                velocity: Vec2::zero(),
            },
            generation: 0,
            speed: 5,
            cell_grid: vec![
                vec![Cell { alive: false }; dimension.y as usize];
                (dimension.x / 2) as usize
            ],
        }
    }

    fn setup(&mut self) {}

    fn update_cell_grid(&mut self) {
        let mut next_grid = self.cell_grid.clone();

        for x in 0..self.cell_grid.len() {
            for y in 0..self.cell_grid[x].len() {
                let neightboors = self.get_neightboor(Vec2::xy(x, y));

                if next_grid[x][y].alive && (neightboors < 2 || neightboors > 3) {
                    next_grid[x][y].alive = false;
                } else if !next_grid[x][y].alive && neightboors == 3 {
                    next_grid[x][y].alive = true;
                }
            }
        }

        self.generation += 1;
        self.cell_grid = next_grid;
    }

    fn update_pointer(&mut self) {
        self.pointer.position.x += self.pointer.velocity.x * 2;
        self.pointer.position.y += self.pointer.velocity.y;

        self.pointer.position.x = self.pointer.position.x.clamp(
            0,
            self.dimension.x - if self.dimension.x % 2 == 0 { 2 } else { 3 },
        );
        self.pointer.position.y = self.pointer.position.y.clamp(0, self.dimension.y - 1);

        self.pointer.velocity = Vec2::zero();
    }

    fn clear_cell_grid(&mut self) {
        for row in self.cell_grid.iter_mut() {
            for cell in row {
                cell.alive = false;
            }
        }
    }

    fn random_cell_grid(&mut self) {
        let mut rng = rand::thread_rng();

        for row in self.cell_grid.iter_mut() {
            for cell in row {
                cell.alive = rng.gen();
            }
        }
    }

    fn get_neightboor(&mut self, position: Vec2) -> i32 {
        let mut neightboors = 0;

        // top middle
        if position.y > 0 {
            if self.cell_grid[(position.x) as usize][(position.y - 1) as usize].alive {
                neightboors += 1;
            }
        }

        // bottom middle
        if position.y < self.dimension.y - 1 {
            if self.cell_grid[(position.x) as usize][(position.y + 1) as usize].alive {
                neightboors += 1;
            }
        }

        // left middle
        if position.x > 0 {
            if self.cell_grid[(position.x - 1) as usize][(position.y) as usize].alive {
                neightboors += 1;
            }
        }

        // right middle
        if position.x < self.dimension.x / 2 - 1 {
            if self.cell_grid[(position.x + 1) as usize][(position.y) as usize].alive {
                neightboors += 1;
            }
        }

        // top left
        if position.y > 0 && position.x > 0 {
            if self.cell_grid[(position.x - 1) as usize][(position.y - 1) as usize].alive {
                neightboors += 1;
            }
        }

        // top right
        if position.y > 0 && position.x < self.dimension.x / 2 - 1 {
            if self.cell_grid[(position.x + 1) as usize][(position.y - 1) as usize].alive {
                neightboors += 1;
            }
        }

        // bottom left
        if position.y < self.dimension.y - 1 && position.x > 0 {
            if self.cell_grid[(position.x - 1) as usize][(position.y + 1) as usize].alive {
                neightboors += 1;
            }
        }

        // bottom right
        if position.y < self.dimension.y - 1 && position.x < self.dimension.x / 2 - 1 {
            if self.cell_grid[(position.x + 1) as usize][(position.y + 1) as usize].alive {
                neightboors += 1;
            }
        }

        neightboors
    }

    fn get_pointer_cell(&mut self) -> &mut Cell {
        let pointer = self.pointer.position;
        &mut self.cell_grid[(pointer.x / 2) as usize][pointer.y as usize]
    }

    // fn play_sound(&mut self, sound: &str) {
    //     self.stream_handle
    //         .play_raw(
    //             Decoder::new(Cursor::new(match sound {
    //                 _ => b"",
    //             }))
    //             .unwrap()
    //             .convert_samples(),
    //         )
    //         .unwrap();
    // }
}

fn main() {
    let mut app = App::config(Config { fps: 60 });
    let mut fps_counter = FPSCounter::default();

    let mut show_infos = false;

    let (_stream, _ /*stream_handle*/) = OutputStream::try_default().unwrap();
    let mut game = Game::new(app.window().size(), /*stream_handle*/);
    game.setup();

    let down_time = 10;
    let mut down_timer = 0;
    let mut down = false;

    app.run(|state: &mut State, window: &mut Window| {
        for key in state.keyboard().last_key_events() {
            match key {
                KeyEvent::Pressed(Key::Esc) => state.stop(),
                KeyEvent::Pressed(Key::Q) => state.stop(),
                KeyEvent::Pressed(Key::F3) => show_infos = !show_infos,
                KeyEvent::Pressed(Key::Space) => match game.scene {
                    Scenes::Start => game.scene = Scenes::Playing,
                    Scenes::Playing => game.scene = Scenes::Paused,
                    Scenes::Paused => game.scene = Scenes::Playing,
                },

                KeyEvent::Pressed(Key::C) => game.clear_cell_grid(),

                KeyEvent::Pressed(Key::N) => game.update_cell_grid(),

                KeyEvent::Pressed(Key::R) => game.random_cell_grid(),

                KeyEvent::Pressed(Key::E) => {
                    let cell = game.get_pointer_cell();
                    cell.alive = !cell.alive;
                }

                KeyEvent::Pressed(Key::RightBracket) => {
                    game.speed += 1;
                    game.speed = game.speed.clamp(1, 10);
                }
                KeyEvent::Pressed(Key::LeftBracket) => {
                    game.speed -= 1;
                    game.speed = game.speed.clamp(1, 10);
                }

                KeyEvent::Pressed(Key::Up | Key::K) => game.pointer.velocity.y = -1,
                KeyEvent::Pressed(Key::Down | Key::J) => game.pointer.velocity.y = 1,
                KeyEvent::Pressed(Key::Left | Key::H) => game.pointer.velocity.x = -1,
                KeyEvent::Pressed(Key::Right | Key::L) => game.pointer.velocity.x = 1,

                KeyEvent::Released(Key::Up | Key::K)
                | KeyEvent::Released(Key::Down | Key::J)
                | KeyEvent::Released(Key::Left | Key::H)
                | KeyEvent::Released(Key::Right | Key::L) => {
                    down_timer = 0;
                    down = false;
                }
                _ => {}
            }
        }

        if state.step() % 2 == 0 {
            for key in state.keyboard().get_keys_down() {
                match key {
                    Key::Up | Key::K => {
                        down_timer += 1;

                        if down || down_timer == down_time {
                            game.pointer.velocity.y = -1;
                            down = true;
                        }
                    }
                    Key::Down | Key::J => {
                        down_timer += 1;

                        if down || down_timer == down_time {
                            game.pointer.velocity.y = 1;
                            down = true;
                        }
                    }
                    Key::Left | Key::H => {
                        down_timer += 1;

                        if down || down_timer == down_time {
                            game.pointer.velocity.x = -1;
                            down = true;
                        }
                    }
                    Key::Right | Key::L => {
                        down_timer += 1;

                        if down || down_timer == down_time {
                            game.pointer.velocity.x = 1;
                            down = true;
                        }
                    }
                    _ => {}
                }
            }
        }

        fps_counter.update();

        game.update_pointer();

        if state.step() % game.speed as usize == 0 {
            match game.scene {
                Scenes::Playing => game.update_cell_grid(),
                _ => {}
            }
        }

        let mut pencil = Pencil::new(window.canvas_mut());

        for x in 0..game.cell_grid.len() {
            for y in 0..game.cell_grid[x].len() {
                if game.cell_grid[x][y].alive {
                    pencil.set_background(Color::White);
                    pencil.set_foreground(Color::White);
                } else {
                    pencil.set_background(Color::Black);
                    pencil.set_foreground(Color::Black);
                }

                pencil.draw_rect(
                    &RectCharset::simple_lines(),
                    Vec2::xy(x * 2, y),
                    Vec2::xy(2, 1),
                );
            }
        }

        if game.get_pointer_cell().alive {
            pencil.set_background(Color::Cyan);
            pencil.set_foreground(Color::Cyan);
        } else {
            pencil.set_background(Color::Blue);
            pencil.set_foreground(Color::Blue);
        }
        pencil.draw_rect(
            &RectCharset::simple_lines(),
            game.pointer.position,
            Vec2::xy(2, 1),
        );

        pencil.set_background(Color::Black);
        pencil.set_foreground(Color::White);
        pencil.draw_text(
            &format!("GEN: {}", game.generation),
            Vec2::xy(2, game.dimension.y - 2),
        );

        let label = "Paused";
        match game.scene {
            Scenes::Start | Scenes::Paused => {
                pencil.set_background(Color::Black);
                pencil.set_foreground(Color::Red);
                pencil.set_style(Style::Bold);
                pencil.draw_text(
                    label,
                    Vec2::xy(
                        game.dimension.x / 2 - label.len() as i32 / 2,
                        game.dimension.y - 2,
                    ),
                );
            }
            _ => {}
        }

        if show_infos {
            pencil.set_background(Color::Black);
            pencil.set_foreground(Color::White);
            pencil.set_style(Style::Plain);
            pencil.draw_text(&format!("FPS: {}", fps_counter.count()), Vec2::xy(2, 1));
            pencil.draw_text(&format!("SPEED: {}", game.speed), Vec2::xy(2, 2));
            pencil.draw_text(&format!("SCENE: {:?}", game.scene), Vec2::xy(2, 3));
            pencil.draw_text(
                &format!(
                    "POINTER: (x:{}, y:{})",
                    game.pointer.position.x / 2,
                    game.pointer.position.y
                ),
                Vec2::xy(2, 4),
            );
            pencil.draw_text(
                &format!(
                    "NEIGHTBOORS: {:?}",
                    game.get_neightboor(Vec2::xy(
                        game.pointer.position.x / 2,
                        game.pointer.position.y
                    ))
                ),
                Vec2::xy(2, 5),
            );
        }
    });
}
