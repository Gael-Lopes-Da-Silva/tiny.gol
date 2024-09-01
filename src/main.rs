use ruscii::app::*;
use ruscii::drawing::*;
use ruscii::gui::*;
use ruscii::keyboard::*;
use ruscii::spatial::*;
use ruscii::terminal::*;

use rand::prelude::*;

fn main() {
    let mut app = App::default();

    app.run(|state: &mut State, window: &mut Window| {
        for key in state.keyboard().last_key_events() {
            match key {
                KeyEvent::Pressed(Key::Esc) => state.stop(),
                KeyEvent::Pressed(Key::Q) => state.stop(),
                _ => {}
            }
        }

        let mut pencil = Pencil::new(window.canvas_mut());

        pencil.set_background(Color::White);
        pencil.draw_filled_rect(' ', Vec2::zero(), Vec2::xy(2, 1));
    });
}
