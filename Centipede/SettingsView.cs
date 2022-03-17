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
        private SpriteFont m_fontMenuSelect;
        Dictionary<Keys, ControlsEnum> controls;
        GameView gameView;
        Dictionary<ControlsEnum, Keys> bindings;
        List<SettingsButton> clickableItems = new List<SettingsButton>();

        Dictionary<ControlsEnum, bool> set = new Dictionary<ControlsEnum, bool>();
        bool setLock = false;
        bool buttonsAdded = false;
        bool error = false;

        public SettingsView(GameView gameView)
        {
            this.gameView = gameView;
            this.controls = gameView.controls;
            bindings = swapDictionary<Keys, ControlsEnum>(controls);

            foreach (ControlsEnum k in bindings.Keys) // for each control
            {
                set.Add(k, false);
            }
        }

        public override void loadContent(ContentManager contentManager)
        {
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menu");
            m_fontMenuSelect = contentManager.Load<SpriteFont>("Fonts/menu-select");
        }

        public override void processInput(GameTime gameTime)
        {
            
            if (setLock)
            {
                KeyboardState keyboard = Keyboard.GetState();
                Keys[] pressedKeys = keyboard.GetPressedKeys();
                foreach (ControlsEnum control in set.Keys)
                {
                    if (set[control] && pressedKeys.Length > 0)
                    {
                        Keys oldKey = bindings[control];
                        bindings[control] = pressedKeys[0];
                        Dictionary<Keys, ControlsEnum> potential = swapDictionary<ControlsEnum, Keys>(bindings);
                        if (potential != null)
                        {
                            set[control] = false;
                            setLock = false;
                            error = false;
                            controls = potential;
                            gameView.controls = potential;
                            break;
                        } else
                        {
                            bindings[control] = oldKey; // reset the control
                            error = true;
                            buttonsAdded = false; // the error will change the string that we're drawing
                        }
                    }
                }
            } else
            {
                MouseState mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    foreach (SettingsButton button in clickableItems)
                    {
                        if (button.clicked(mouseState.X, mouseState.Y))
                        {
                            if (button.back)
                            {
                                gameStateStack.Pop();
                            } else
                            {
                                setControl(button.control);
                            }
                            break;
                        }
                    }
                }
            }
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            // calculate a quarter of the screen
            int height = m_graphics.GraphicsDevice.Viewport.Bounds.Height;

            // draw options
            float bottom = drawControlItem(height / 4, ControlsEnum.up);
            bottom = drawControlItem(bottom, ControlsEnum.right);
            bottom = drawControlItem(bottom, ControlsEnum.down);
            bottom = drawControlItem(bottom, ControlsEnum.left);
            drawBackButton(bottom);
            buttonsAdded = true;

            m_spriteBatch.End();
        }

        private float drawControlItem(float y, ControlsEnum control)
        {
            String text;
            SpriteFont font;
            Color color;
            if (set[control])
            {

                text = "Move " + control.ToString() + ": " + (error ? "Press a key that isn't used" : "Press key to bind");
                font = m_fontMenuSelect;
                color = Color.Yellow;
                
            } else
            {
                text = "Move " + control.ToString() + ": " + bindings[control].ToString();
                font = m_fontMenu;
                color = Color.Blue;
            }

            Vector2 stringSize = font.MeasureString(text);
            Vector2 position = new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, y);

            m_spriteBatch.DrawString(
                font,
                text,
                position,
                color);

            if (!buttonsAdded)
                clickableItems.Add(new SettingsButton(new Rectangle((int)position.X, (int)position.Y, (int)stringSize.X, (int)stringSize.Y), control, false));

            return y + stringSize.Y;
        }

        private void drawBackButton(float y)
        {
            Vector2 stringSize = m_fontMenu.MeasureString("Back");
            Vector2 position = new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, y);
            m_spriteBatch.DrawString(m_fontMenu, "Back", position, Color.Blue);
            clickableItems.Add(new SettingsButton(new Rectangle((int)position.X, (int)position.Y, (int)stringSize.X, (int)stringSize.Y), ControlsEnum.up, true));
        }

        public void setControl(ControlsEnum control)
        {
            if (!setLock)
            {
                set[control] = true;
                setLock = true;
                buttonsAdded = false; // we will need to redraw the rectangles to compensate for the shift in size
                clickableItems.Clear();
            }
        }

        private Dictionary<TValue, TKey> swapDictionary<TKey, TValue>(Dictionary<TKey, TValue> source)
        {
            Dictionary<TValue, TKey> result = new Dictionary<TValue, TKey>();
            foreach (var entry in source)
            {
                if(!result.ContainsKey(entry.Value)) {
                    result.Add(entry.Value, entry.Key); // in case there are duplicate values in the original (should't be a problem here)
                } else
                {
                    return null;
                }
            }
            return result;
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}
