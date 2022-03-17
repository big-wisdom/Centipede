using CS5410;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using static Centipede.GameView;

namespace Centipede
{
    class SettingsView : GameStateView
    {
        private SpriteFont m_fontMenu;
        Dictionary<Keys, ControlsEnum> controls;
        Dictionary<ControlsEnum, Keys> bindings;

        public SettingsView(Dictionary<Keys, ControlsEnum> controls)
        {
            this.controls = controls;
            bindings = swapDictionary<Keys, ControlsEnum>(controls);
        }

        public override void loadContent(ContentManager contentManager)
        {
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menu");
        }

        public override void processInput(GameTime gameTime)
        {
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            // calculate a quarter of the screen
            int height = m_graphics.GraphicsDevice.Viewport.Bounds.Height;

            // draw options
            float bottom = drawMenuItem(m_fontMenu, "Move Up: "+bindings[ControlsEnum.up].ToString() , height / 4, Color.Blue);
            bottom = drawMenuItem(m_fontMenu, "Move Right: " + bindings[ControlsEnum.right].ToString(), bottom, Color.Blue);
            bottom = drawMenuItem(m_fontMenu, "Move Down: " + bindings[ControlsEnum.down].ToString(), bottom, Color.Blue);
            drawMenuItem(m_fontMenu, "Move Left: " + bindings[ControlsEnum.left].ToString(), bottom, Color.Blue);

            m_spriteBatch.End();
        }

        private float drawMenuItem(SpriteFont font, string text, float y, Color color)
        {
            Vector2 stringSize = font.MeasureString(text);
            m_spriteBatch.DrawString(
                font,
                text,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, y),
                color);

            return y + stringSize.Y;
        }

        private Dictionary<TValue, TKey> swapDictionary<TKey, TValue>(Dictionary<TKey, TValue> source)
        {
            Dictionary<TValue, TKey> result = new Dictionary<TValue, TKey>();
            foreach (var entry in source)
            {
                if(!result.ContainsKey(entry.Value)) {
                    result.Add(entry.Value, entry.Key); // in case there are duplicate values in the original (should't be a problem here)
                }
            }
            return result;
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}
