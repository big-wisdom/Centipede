using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Centipede;
using static Centipede.GameView;

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
            Vector2 shipPosition = new Vector2(m_graphics.GraphicsDevice.Viewport.Bounds.Width / 2, 7 * (m_graphics.GraphicsDevice.Viewport.Bounds.Height / 8));
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
            Dictionary<Entity, List<Collision>> collisions = collisionDetection(gametime); // I'll get the dictionary here

            // And in the section, while updating each object, notify it of it's collisions
            // and allow it to react properly
            ship.update(gametime, collisions[ship]);
        }

        // I should have this return a dictionary whose key is the object and whose
        // value is a list of collision objects
        private Dictionary<Entity, List<Collision>> collisionDetection(GameTime time) {
            Dictionary<Entity, List<Collision>> result = new Dictionary<Entity, List<Collision>>();
            // get pairs of colliding objects
            // check ship against walls
            Rectangle walls = m_graphics.GraphicsDevice.Viewport.Bounds;
            Rectangle shipBoundary = new Rectangle(0, (walls.Height*3)/4, walls.Width, walls.Height/4);
            result.Add(ship, ship.checkBoundaryCollision(shipBoundary, time));

            // add the collision for both objects

            return result;
        }
    }
}
