using System.Collections.Generic;
using CS5410;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Centipede
{
    public class ScreenController
    {
        public IGameState m_currentState {
            get {
                return m_states[viewStack.Peek()];
            }
        }

        public GameStateEnum m_currentStateType { 
            get {
                return viewStack.Peek();
            }
        }

        public Dictionary<GameStateEnum, IGameState> m_states { get; set; }
        private Stack<GameStateEnum> viewStack = new Stack<GameStateEnum>();
        public KeyboardModel keyboard = new KeyboardModel();

        public ScreenController()
        {
            GameView gameView = new GameView();
            m_states = new Dictionary<GameStateEnum, IGameState>();
            m_states.Add(GameStateEnum.MainMenu, new MainMenuView());
            m_states.Add(GameStateEnum.GamePlay, gameView);
            m_states.Add(GameStateEnum.Settings, new SettingsView(gameView));

            // We are starting with the main menu
            viewStack.Push(GameStateEnum.MainMenu);
        }

        public void initializeStates(GraphicsDevice graphicsDevice, GraphicsDeviceManager gdm, ContentManager content) { 
            foreach (var item in m_states)
            {
                item.Value.initialize(graphicsDevice, gdm, viewStack, keyboard);
                item.Value.loadContent(content);
            }
        }
    }
}