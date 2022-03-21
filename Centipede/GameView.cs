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

        public enum CharachterEnum
        {
            Ship,
            Centipede,
            Mushroom,
            Spider,
            laser,
            topWall,
            rightWall,
            bottomWall,
            leftWall
        }

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
        Texture2D spriteSheet;
        Texture2D overlayTexture;

        // overlay variables
        Boolean paused = false;
        Overlay overlay;

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

        private void resetGame() {
            paused = false;
            overlay = new Overlay();
            game = new Centipede();
        }

        public override void loadContent(ContentManager contentManager)
        {
            m_fontMenu = contentManager.Load<SpriteFont>(scaler.m_fontMenu);
            m_fontMenuSelect = contentManager.Load<SpriteFont>(scaler.m_fontMenuSelect);

            spriteSheet = contentManager.Load<Texture2D>("spriteSheet");

            overlayTexture = new Texture2D(m_graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            overlayTexture.SetData(new[] { Color.DarkBlue });

            resetGame();
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
                            resetGame();
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

            // render cenipede
            foreach (Entity e in game.centipede.centipede) {
                drawEntity(e);
            }

            // draw pause overlay
            if (paused) {
                drawPauseOverlay();
            }

            m_spriteBatch.End();
        }


        // This method is written with the assumption that the model is build around my
        // "normal sized screen". So scaling the screen to it's true size will be left to this method
        private void drawEntity(Entity e)
        {
            Rectangle source = e.computeSourceRectangle();
            Vector2 scaledRenderPosition = Vector2.Multiply(e.renderPosition, scaler.screenScaleRatio);
            //Rectangle destination = new Rectangle((int)scaledRenderPosition.X, (int)scaledRenderPosition.Y, (int)(source.Width * scaler.screenScaleRatio), (int)(source.Height * scaler.screenScaleRatio));

            //m_spriteBatch.Draw(spriteSheet, destination, source, Color.White);
            // I think I should make screenScaleRatio a vector
            SpriteEffects effect = e.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            m_spriteBatch.Draw(spriteSheet, scaledRenderPosition, source, Color.White, 0, Vector2.Zero, scaler.screenScaleRatio, effect, 0);
        }

        private void drawPauseOverlay() {
            // I want this to be half the width and half the height of the screen in the center
            Rectangle bounds = m_graphics.GraphicsDevice.Viewport.Bounds;
            // Vector2 position = new Vector2(bounds.Width / 4, bounds.Height / 4);
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
            Color color = overlay.selected == selected ? Color.Yellow : Color.Orange;
            SpriteFont font = overlay.selected == selected ? m_fontMenuSelect : m_fontMenu;
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
