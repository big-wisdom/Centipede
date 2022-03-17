using System;
using System.Collections.Generic;
using CS5410;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Centipede
{
    public class GameView : GameStateView
    {
        Centipede game = new Centipede();
        Stack<GameStateEnum> gameStateStack;

        SpriteFont m_fontMenu;

        public GameView(Stack<GameStateEnum> gameStateStack)
        {
            this.gameStateStack = gameStateStack;
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

            m_spriteBatch.DrawString(
                m_fontMenu,
                "Hello World",
                new Vector2(m_graphics.PreferredBackBufferWidth / 2, m_graphics.PreferredBackBufferHeight / 2),
                Color.Aqua);

            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}
