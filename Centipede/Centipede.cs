using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Centipede;
using static Centipede.GameView;

namespace Centipede
{
    public class Centipede
    {
        int numberOfMushrooms = 30;
        public bool gameOver = false;
        // set a standard screen size and in rendering I will account for bigger or smaller screens
        private Rectangle bounds = new Rectangle(0, 0, 1280, 800);

        public Ship ship { get; set; }
        public CentipedeCharachter centipede { get; set; }
        public List<Mushroom> mushrooms = new List<Mushroom>();
        public List<Laser> lasers = new List<Laser>();

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

            // some test shrooms
            //Mushroom m = new Mushroom(1, 1);
            //mushrooms.Add(m);
            //Mushroom mu = new Mushroom(38, 1);
            //mushrooms.Add(mu);

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

        TimeSpan coolDownTime = TimeSpan.FromMilliseconds(200);
        TimeSpan coolDown = TimeSpan.FromMilliseconds(200);
        public void shoot()
        {
            if (coolDown <= TimeSpan.Zero)
            {
                // if shoot timer allows it
                // calculate position
                Vector2 laserPosition = ship.position + new Vector2(0, -28);
                // create laser and add it to list
                lasers.Add(new Laser(laserPosition));
                coolDown = coolDownTime;
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
            // if ship is dead, game over
            if (ship.dead)
            {
                gameOver = true;
            }

            if (!gameOver)
            {
                coolDown -= gameTime.ElapsedGameTime;

                Dictionary<Entity, List<Collision>> collisions = collisionDetection(gameTime); // I'll get the dictionary here

                // And in the section, while updating each object, notify it of it's collisions
                // and allow it to react properly
                if (collisions.ContainsKey(ship))
                {
                    ship.update(gameTime, collisions[ship]);
                }
                else
                {
                    ship.update(gameTime, new List<Collision>());
                }

                List<Laser> removeLasers = new List<Laser>();
                foreach (Laser l in lasers)
                {
                    if (l.exists)
                    {
                        l.update(gameTime, new List<Collision>());
                    } else
                    {
                        removeLasers.Add(l);
                    }
                }
                foreach (Laser l in removeLasers) lasers.Remove(l);

                List<Mushroom> remove = new List<Mushroom>();
                foreach(Mushroom m in mushrooms)
                {
                    if (collisions.ContainsKey(m)) m.update(gameTime, collisions[m]);
                    if (!m.alive) remove.Add(m);
                }
                foreach (Mushroom m in remove) mushrooms.Remove(m);

                List<CentipedeSegment> deadSegments = centipede.removeDeadSegments();
                turnDeadSegmentsIntoMushrooms(deadSegments);
                foreach (CentipedeSegment s in centipede.centipede)
                {
                    s.update(gameTime, collisions[s]);
                }
            }

        }

        private void turnDeadSegmentsIntoMushrooms(List<CentipedeSegment> dead)
        {
            foreach (CentipedeSegment s in dead)
            {
                // calculate which box they were in
                int x = (int)Math.Floor(s.position.X / 32);
                int y = (int)Math.Floor(s.position.Y / 32);
                // create a mushroom there
                mushrooms.Add(new Mushroom(x, y));
            }
        }

        // I should have this return a dictionary whose key is the object and whose
        // value is a list of collision objects
        private Dictionary<Entity, List<Collision>> collisionDetection(GameTime time) {
            Dictionary<Entity, List<Collision>> result = new Dictionary<Entity, List<Collision>>();

            if (!ship.hit) {             
                // check ship against centipede
                foreach (CentipedeSegment c in centipede.centipede)
                {
                    Collision collision = ship.checkForCollision(c, time.ElapsedGameTime);
                    if (collision != null)
                    {
                        if (result.ContainsKey(ship))
                        {
                            result[ship].Add(collision);
                        } else
                        {
                            result.Add(ship, new List<Collision> { collision });
                        }
                    }
                }

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
                Rectangle shipBoundary = new Rectangle(0, (walls.Height * 3) / 4, walls.Width, walls.Height / 4);
                List<Collision> wallCollisions = ship.checkBoundaryCollision(shipBoundary, time);
                if (result.ContainsKey(ship))
                {
                    result[ship].AddRange(wallCollisions);
                } else
                {
                    result.Add(ship, wallCollisions);
                }
            }

            // check lasers against centipede and mushrooms
            foreach (Laser l in lasers)
            {
                foreach (CentipedeSegment s in centipede.centipede)
                {
                    // create the collision from the point of view of the centipede segment so it reacts to it easier
                    // and set exists to false on the laser so that it can be cleaned up in update
                    Collision collision = s.checkForCollision(l, time.ElapsedGameTime);
                    if (collision != null)
                    {
                        l.exists = false;
                        if (result.ContainsKey(s)) result[s].Add(collision);
                        else result.Add(s, new List<Collision> { collision });
                    }
                }

                foreach (Mushroom m in mushrooms)
                {
                    Collision collision = m.checkForCollision(l, time.ElapsedGameTime);
                    if (collision != null)
                    {
                        l.exists = false;
                        if (result.ContainsKey(m)) result[m].Add(collision);
                        else result.Add(m, new List<Collision> { collision });
                    }
                }
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

                foreach (Mushroom m in mushrooms)
                {
                    Collision c = s.checkForCollision(m, time.ElapsedGameTime);
                    if (c != null) collisions.Add(c);
                }

                // check against all mushrooms
            }

            // add the collision for both objects
            return result;
        }
    }
}
