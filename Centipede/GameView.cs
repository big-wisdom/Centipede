using System;
using System.Collections.Generic;
using CS5410;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Centipede
{
    public class GameView : GameStateView
    {
        Centipede game;
        Stack<GameStateEnum> gameStateStack;

        // Rendering info configuration to be passed into centipede object, but the textures loaded here
        // I realized that some rendering info is necessary inside the gameModel, like radius is tied to the texture
        // that I use. So content will be loaded here and passed into the model, then the model will be rendered here.
        // this enum dictionary act like a config
        public enum CharachterEnum
        {
            Ship,
            Centipede,
            Mushroom,
            Spider,
            laser
        }

        Dictionary<CharachterEnum, Dictionary<String, dynamic>> charachters = new Dictionary<CharachterEnum, Dictionary<string, dynamic>>
        {
            { CharachterEnum.Ship,
                new Dictionary<string, dynamic>{
                    {"radius" , 20 },
                    {"maxSpeed", 300}, // per second
                    { "texturePath", "spriteSheets/ship" },
                    { "renderOffset", new Vector2(-14, -16) },
                }
            }
        };

        
        // This enum and dictionary are config for the controls
        public enum ControlsEnum
        {
            up,
            right,
            down,
            left
        }

        public Dictionary<Keys, ControlsEnum> controls { get; set; }

        SpriteFont m_fontMenu;
        Texture2D ship;

        public GameView(Stack<GameStateEnum> gameStateStack)
        {
            this.gameStateStack = gameStateStack;

            controls = new Dictionary<Keys, ControlsEnum>
            {
                {Keys.Up, ControlsEnum.up},
                {Keys.Right, ControlsEnum.right},
                {Keys.Down, ControlsEnum.down},
                {Keys.Left, ControlsEnum.left}
            };
        }

        public override void loadContent(ContentManager contentManager)
        {
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menu");

            ship = contentManager.Load<Texture2D>("spriteSheets/ship");
            charachters[CharachterEnum.Ship]["texture"] = ship;

            game = new Centipede(charachters, m_graphics);
        }

        public override void processInput(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            Keys[] keysPressed = keyboard.GetPressedKeys();
            double y = 0;
            double x = 0;
            bool move = false;
            foreach(Keys k in keysPressed) {
                if (controls.ContainsKey(k))
                {
                    ControlsEnum control = controls[k];
                    if (control == ControlsEnum.left)
                    {
                        x -= 1;
                        move = true;
                    }
                    if (control == ControlsEnum.right)
                    {
                        x += 1;
                        move = true;
                    }
                    if (control == ControlsEnum.up)
                    {
                        y -= 1;
                        move = true;
                    }
                    if (control == ControlsEnum.down)
                    {
                        y += 1;
                        move = true;
                    }
                }
            }
            double angle = Math.Atan2(y, x);

            if (move)
            {
                game.moveShip(angle);
            } else
            {
                game.stopShip();
            }

            game.update(gameTime);
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            //m_spriteBatch.DrawString(
            //    m_fontMenu,
            //    "Hello World",
            //    new Vector2(m_graphics.PreferredBackBufferWidth / 2, m_graphics.PreferredBackBufferHeight / 2),
            //    Color.Aqua);

            // render ship
            drawEntity(game.ship);

            m_spriteBatch.End();
        }

        private void drawEntity(Entity e)
        {
            CharachterEnum type = e.type;
            Vector2 offset = charachters[type]["renderOffset"];
            Texture2D texture = charachters[type]["texture"];
            m_spriteBatch.Draw(texture, game.ship.position + offset, Color.White);
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}
