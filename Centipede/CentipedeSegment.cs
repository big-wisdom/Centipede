using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using static Centipede.GameView;

namespace Centipede
{
    public class CentipedeSegment : Entity
    {
        Rectangle bigWalls;
        double previousAngle;
        TimeSpan holdTime = TimeSpan.Zero;
        private int frame;
        private TimeSpan animationMilliseconds = TimeSpan.FromMilliseconds(25);
        private TimeSpan timeTillAnimate = TimeSpan.FromMilliseconds(25);

        int row = 1;
        public Rectangle walls {
            get {
                return new Rectangle(0, 0, bigWalls.Width, row * 32);
            }
        }

        public override bool flip => computeFlip();

        private bool computeFlip() {
            // this assumes that only positive angles will be given
            double adjusted = angle % (2 * Math.PI); 
            if (15 * Math.PI / 8 > adjusted && adjusted < 5* Math.PI / 8) return true; // pretty much everything right of up
            // if (11 * Math.PI / 8 < adjusted && adjusted > 7 * Math.PI / 8) return false; // everything left of up
            return false;
        }

        public CentipedeSegment(Vector2 position, Vector2 offset, double angle, int startingFrame, Rectangle walls): base(position, offset, 16, 500, CharachterEnum.Centipede)
        {
            previousAngle = angle;
            move(angle);
            this.bigWalls = walls;
            frame = startingFrame;
        }

        public override Rectangle computeSourceRectangle()
        {
            int x = 0;
            int y = 64;
            int cell = 32;
            if (frame % 2 == 0)
            {
                x += (frame / 2) * cell;
            } else
            {
                y += cell;
                x += ((frame - 1) / 2) * cell;
            }

            return new Rectangle(x, y, 32, 32);
        }

        public override void update(GameTime gameTime, List<Collision> collisions)
        {
            updateFrame(gameTime);

            // if moving down
                // if you hit a mushroom or a wall
                    // if you can go the opposite direction
                        // go opposite direction
                    // else
                        // go the same direction
            // if moving to the side
                // if you hit a mushroom or a wall
                    // if you can go down
                        // go down
                    // else
                        // turn around

            // if moving down
                // if you hit a mushroom or a wall
                    // collide with it
                    // try going left


            foreach (Collision c in collisions) {
                switch (c.entityType) {
                    case CharachterEnum.Mushroom:
                        if (3*Math.PI/8 < angle&&angle < 5*Math.PI/8)
                        {
                            goto case CharachterEnum.bottomWall;
                        } else
                        {
                            goto case CharachterEnum.leftWall;
                        }
                    case CharachterEnum.rightWall:
                        goto case CharachterEnum.leftWall;
                    case CharachterEnum.leftWall:
                        row++;
                        move(Math.PI / 2);
                        // snap into grid
                        position += c.toImpact;
                        // set hold time
                        holdTime = TimeSpan.FromSeconds(Math.Abs(c.toImpact.X / (float)maxSpeed));
                        break;

                    case CharachterEnum.bottomWall:
                        previousAngle = (previousAngle + Math.PI) % (2 * Math.PI);
                        move(previousAngle);
                        position += c.toImpact;
                        holdTime = TimeSpan.FromSeconds(Math.Abs(c.toImpact.Y / (float)maxSpeed));
                        break;
                }
            }

            // here I adjust for snapping to the wall that is impacted
            TimeSpan adjustedTime = gameTime.ElapsedGameTime - holdTime;
            if (adjustedTime.Seconds < 0)
            {
                holdTime = TimeSpan.FromSeconds(Math.Abs(adjustedTime.Seconds)); // if there's left over holdTime, leave it for next cycle
            } else
            {
               holdTime = TimeSpan.Zero;
               position = getNextPosition(adjustedTime);
            }
        }

        private void updateFrame(GameTime gameTime)
        {
            timeTillAnimate -= gameTime.ElapsedGameTime;
            if (timeTillAnimate.TotalMilliseconds < 0)
            {
                timeTillAnimate = animationMilliseconds + timeTillAnimate;
                frame = (frame + 1) % 8;
            }
        }
    }
}
