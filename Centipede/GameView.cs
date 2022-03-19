using System;
using System.Collections.Generic;
using CS5410;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Centipede.Overlay;

namespace Centipede
{
    public class GameView : GameStateView
    {
        Centipede game;

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
        SpriteFont m_fontMenuSelect;
        Texture2D ship;
        Texture2D overlayTexture;

        // overlay variables
        Boolean paused = false;
        Overlay overlay = new Overlay();

        public GameView()
        {
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
            m_fontMenuSelect = contentManager.Load<SpriteFont>("Fonts/menu-select");

            ship = contentManager.Load<Texture2D>("spriteSheets/ship");
            charachters[CharachterEnum.Ship]["texture"] = ship;

            overlayTexture = new Texture2D(m_graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            overlayTexture.SetData(new[] { Color.DarkBlue });

            game = new Centipede(charachters, m_graphics);
        }

        public override void processInput(GameTime gameTime)
        {
            List<Keys> unlockedKeys = keyboard.GetUnlockedKeys();
            double y = 0;
            double x = 0;
            bool move = false;
            foreach(Keys k in unlockedKeys) {
                if (paused) { 
                    if (k == Keys.Down) {
                        overlay.down();
                        keyboard.lockKey(k);
                    } else if (k == Keys.Up) {
                        overlay.up();
                        keyboard.lockKey(k);
                    } else if (k == Keys.Enter) { 
                        if (overlay.selected == OverlayOptions.Continue) {
                            paused = false;
                        } else if (overlay.selected == OverlayOptions.Quit) {
                            gameStateStack.Pop();
                        }
                        keyboard.lockKey(k);
                    }

                }
                else
                {
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
                    if (k == Keys.Escape)
                    {
                        paused = true;
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

            // render ship
            drawEntity(game.ship);

            // draw pause overlay
            if (paused) {
                drawPauseOverlay();
            }

            m_spriteBatch.End();
        }

        private void drawEntity(Entity e)
        {
            CharachterEnum type = e.type;
            Vector2 offset = charachters[type]["renderOffset"];
            Texture2D texture = charachters[type]["texture"];
            m_spriteBatch.Draw(texture, game.ship.position + offset, Color.White);
        }

        private void drawPauseOverlay() {
            // I want this to be half the width and half the height of the screen in the center
            Rectangle bounds = m_graphics.GraphicsDevice.Viewport.Bounds;
            Vector2 position = new Vector2(bounds.Width / 4, bounds.Height / 4);
            Rectangle rec = new Rectangle(bounds.Width / 4, bounds.Height / 4, bounds.Width / 2, bounds.Height / 2);
            m_spriteBatch.Draw(overlayTexture, rec, Color.Aqua);

            int continue_y = (bounds.Height * 3)/8;
            int quit_y = (bounds.Height * 4) / 8;
            // menu items
            drawMenuItem(continue_y, OverlayOptions.Continue);
            drawMenuItem(quit_y, OverlayOptions.Quit);
        }

        private float drawMenuItem(float y, OverlayOptions selected)
        {
            String text = selected.ToString();
            Color color = overlay.selected == selected ? Color.Orange : Color.Yellow;
            SpriteFont font = overlay.selected == selected ? m_fontMenu : m_fontMenuSelect;
            Vector2 stringSize = font.MeasureString(text);
            m_spriteBatch.DrawString(
                font,
                text,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, y),
                color);

            return y + stringSize.Y;
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}
