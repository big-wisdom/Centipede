using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Centipede;
using static Centipede.GameView;

namespace Centipede
{
    public class Centipede
    {
        int numberOfMushrooms = 20;
        // set a standard screen size and in rendering I will account for bigger or smaller screens
        private Rectangle bounds = new Rectangle(0, 0, 1280, 800);

        public Ship ship { get; set; }
        public CentipedeCharachter centipede { get; set; }
        public List<Mushroom> mushrooms = new List<Mushroom>();

        public Centipede()
        {

            // initialize ship
            Vector2 shipPosition = new Vector2(bounds.Width / 2, 7 * (bounds.Height / 8));
            ship = new Ship(shipPosition);

            // initialize centipede
            int cellSize = 32; // this is for the "normal screen size" scaling is done during rendering
            Random rnd = new Random();
            int index = rnd.Next(11, 29);
            Vector2 centipedePosition = new Vector2(index*cellSize, 0);
            Vector2 offSet = new Vector2(-cellSize/2, -cellSize/2);
            centipede = new CentipedeCharachter(centipedePosition-offSet, offSet, cellSize, bounds);

            for (int i = 0; i < numberOfMushrooms; i++)
            {
                bool added = false;
                while (!added)
                {
                    // initialize mushrooms at random locations
                    int x = rnd.Next(40);
                    int y = rnd.Next(25);
                    // create mushroom
                    Mushroom m = new Mushroom(x, y);
                    // check if it collides with ship or centipede
                    bool spotTaken = false;
                    foreach (CentipedeSegment c in centipede.centipede)
                    {
                        if (m.checkForCollision(c, TimeSpan.Zero) != null)
                        {
                            spotTaken = true;
                        }
                    }
                    if (m.checkForCollision(ship, TimeSpan.Zero) != null)
                    {
                        spotTaken = true;
                    }
                    // add it to the list if it doesn't
                    if (!spotTaken)
                    {
                        mushrooms.Add(m);
                        added = true;
                    }
                }
            }
        }

        public void moveShip(double angle)
        {
            ship.move(angle);
        }

        public void stopShip()
        {
            ship.stop();
        }

        public void update(GameTime gameTime)
        {
            Dictionary<Entity, List<Collision>> collisions = collisionDetection(gameTime); // I'll get the dictionary here

            // And in the section, while updating each object, notify it of it's collisions
            // and allow it to react properly
            ship.update(gameTime, collisions[ship]);

            foreach (CentipedeSegment s in centipede.centipede) {
                s.update(gameTime, collisions[s]);
            }
        }

        // I should have this return a dictionary whose key is the object and whose
        // value is a list of collision objects
        private Dictionary<Entity, List<Collision>> collisionDetection(GameTime time) {
            Dictionary<Entity, List<Collision>> result = new Dictionary<Entity, List<Collision>>();
            // check ship against all mushrooms
            foreach (Mushroom m in mushrooms)
            {
                Collision c = ship.checkForCollision(m, time.ElapsedGameTime);
                if (c != null)
                {
                    if (result.ContainsKey(ship))
                    {
                        result[ship].Add(c);
                    }
                    else
                    {
                        result.Add(ship, new List<Collision> { c });
                    }
                }

            }
            // check ship against walls
            Rectangle walls = bounds;
            Rectangle shipBoundary = new Rectangle(0, (walls.Height*3)/4, walls.Width, walls.Height/4);
            List<Collision> wallCollisions = ship.checkBoundaryCollision(shipBoundary, time);
            if (result.ContainsKey(ship))
            {
                result[ship].AddRange(wallCollisions);
            } else
            {
                result.Add(ship, wallCollisions);
            }



            // check centipede against walls
            foreach (CentipedeSegment s in centipede.centipede) {
                List<Collision> collisions = s.checkBoundaryCollision(s.walls, time);
                if (result.ContainsKey(s))
                {
                    result[s].AddRange(collisions);
                } else
                {
                    result.Add(s, collisions);
                }

                // check against all mushrooms
            }

            // add the collision for both objects
            return result;
        }
    }
}
