using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CS5410
{
    public class MainMenuView : GameStateView
    {
        private SpriteFont m_fontMenu;
        private SpriteFont m_fontMenuSelect;


        private enum MenuState
        {
            NewGame,
            HighScores,
            Help,
            About,
            Quit
        }

        private MenuState m_currentSelection {
            get
            {
                return (MenuState)(currentSelectionIndex % enumLength);
            }
        }

        private int currentSelectionIndex = 0;
        private int enumLength = Enum.GetNames(typeof(MenuState)).Length;

        private bool m_waitForKeyRelease = false;

        public override void loadContent(ContentManager contentManager)
        {
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menu");
            m_fontMenuSelect = contentManager.Load<SpriteFont>("Fonts/menu-select");
        }

        public override void processInput(GameTime gameTime)
        {
            foreach (Keys k in keyboard.GetUnlockedKeys()) { 
                if (k == Keys.Down)
                {
                    currentSelectionIndex += 1;
                    keyboard.lockKey(k);
                }
                if (k == Keys.Up)
                {
                    currentSelectionIndex -= 1;
                    keyboard.lockKey(k);
                    if (currentSelectionIndex == -1) currentSelectionIndex = enumLength - 1;
                }
                
                // If enter is pressed, return the appropriate new state
                if (k == Keys.Enter)
                {
                    keyboard.lockKey(k);
                    if (m_currentSelection == MenuState.NewGame)
                    {
                        gameStateStack.Push(GameStateEnum.GamePlay);
                    }
                    else if (m_currentSelection == MenuState.HighScores)
                    {
                        gameStateStack.Push(GameStateEnum.HighScores);
                    }
                    else if (m_currentSelection == MenuState.Help)
                    {
                        gameStateStack.Push(GameStateEnum.Settings);
                    }
                    else if (m_currentSelection == MenuState.About)
                    {
                        gameStateStack.Push(GameStateEnum.About);
                    }
                    else if (m_currentSelection == MenuState.Quit)
                    {
                        gameStateStack.Push(GameStateEnum.Exit);
                    }
                }
            }
        }


        public override void update(GameTime gameTime)
        {
        }


        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            // calculate a quarter of the screen
            int height =  m_graphics.GraphicsDevice.Viewport.Bounds.Height;
            // I split the first one's parameters on separate lines to help you see them better
            float bottom = drawMenuItem(
                m_currentSelection == MenuState.NewGame ? m_fontMenuSelect : m_fontMenu, 
                "New Game",
                height / 4, 
                m_currentSelection == MenuState.NewGame ? Color.Yellow : Color.Blue
            );
            bottom = drawMenuItem(m_currentSelection == MenuState.HighScores ? m_fontMenuSelect : m_fontMenu, "High Scores", bottom, m_currentSelection == MenuState.HighScores ? Color.Yellow : Color.Blue);
            bottom = drawMenuItem(m_currentSelection == MenuState.Help ? m_fontMenuSelect : m_fontMenu, "Settings", bottom, m_currentSelection == MenuState.Help ? Color.Yellow : Color.Blue);
            bottom = drawMenuItem(m_currentSelection == MenuState.About ? m_fontMenuSelect : m_fontMenu, "About", bottom, m_currentSelection == MenuState.About ? Color.Yellow : Color.Blue);
            drawMenuItem(m_currentSelection == MenuState.Quit ? m_fontMenuSelect : m_fontMenu, "Quit", bottom, m_currentSelection == MenuState.Quit ? Color.Yellow : Color.Blue);

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
    }
}