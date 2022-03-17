using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CS5410
{
    public abstract class GameStateView : IGameState
    {
        protected GraphicsDeviceManager m_graphics;
        protected SpriteBatch m_spriteBatch;
        protected Stack<GameStateEnum> gameStateStack;

        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Stack<GameStateEnum> gameStateStack)
        {
            m_graphics = graphics;
            m_spriteBatch = new SpriteBatch(graphicsDevice);
            this.gameStateStack = gameStateStack;
        }
        public abstract void loadContent(ContentManager contentManager);
        public abstract void processInput(GameTime gameTime);
        public abstract void render(GameTime gameTime);
        public abstract void update(GameTime gameTime);
    }
}
