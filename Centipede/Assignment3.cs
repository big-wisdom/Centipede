using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Centipede;

namespace CS5410
{
    public class Assignment3 : Game
    {
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spriteBatch;

        private ScreenController screenController;

        public Assignment3()
        {
            m_graphics = new GraphicsDeviceManager(this);
            screenController = new ScreenController();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            m_graphics.PreferredBackBufferWidth = 1280;
            m_graphics.PreferredBackBufferHeight = 800;
            m_graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            screenController.initializeStates(this.GraphicsDevice, m_graphics, this.Content);
        }

        protected override void Update(GameTime gameTime)
        {
            // Special case for exiting the game
            screenController.m_currentState.processInput(gameTime); // this has potential to update m_currenState

            if (screenController.m_currentStateType == GameStateEnum.Exit)
            {
                Exit();
            } else { 
                screenController.m_currentState.update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            screenController.m_currentState.render(gameTime);

            base.Draw(gameTime);
        }
    }
}
