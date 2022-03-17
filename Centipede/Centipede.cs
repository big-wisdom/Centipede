using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Centipede;

namespace Centipede
{
    public class Centipede
    {
        Dictionary<GameView.CharachterEnum, Dictionary<String, dynamic>> charachters;
        GraphicsDeviceManager m_graphics;

        public Ship ship { get; set; }
        public Centipede(Dictionary<GameView.CharachterEnum, Dictionary<String, dynamic>> charachters, GraphicsDeviceManager m_graphics)
        {
            this.charachters = charachters;
            this.m_graphics = m_graphics;

            // initialize ship
            Vector2 shipPosition = new Vector2(m_graphics.GraphicsDevice.Viewport.Bounds.Width / 2, 3 * (m_graphics.GraphicsDevice.Viewport.Bounds.Height / 4));
            ship = new Ship(shipPosition, charachters[GameView.CharachterEnum.Ship]["radius"], charachters[GameView.CharachterEnum.Ship]["maxSpeed"], GameView.CharachterEnum.Ship);
        }

        public void moveShip(double angle)
        {
            ship.move(angle);
        }

        public void stopShip()
        {
            ship.stop();
        }

        public void update(GameTime gametime)
        {
            ship.update(gametime);
        }
    }
}
