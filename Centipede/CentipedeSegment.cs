﻿using System;
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

        public CentipedeSegment(Vector2 position, Vector2 offset, double angle, int startingFrame, Rectangle walls): base(position, offset, 16, 300, CharachterEnum.Centipede)
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
            frame = (frame + 1) % 8;
            foreach (Collision c in collisions) {
                switch (c.entityType) {
                    case CharachterEnum.rightWall:
                        goto case CharachterEnum.leftWall;
                    case CharachterEnum.leftWall:
                        row++;
                        move(Math.PI / 2);
                        // snap into grid
                        position += new Vector2(c.distance, 0);
                        // set hold time
                        holdTime = TimeSpan.FromSeconds(Math.Abs((float)c.distance / (float)maxSpeed));
                        break;

                    case CharachterEnum.bottomWall:
                        previousAngle = (previousAngle + Math.PI) % (2 * Math.PI);
                        move(previousAngle);
                        position += new Vector2(0, c.distance);
                        holdTime = TimeSpan.FromSeconds(c.distance / (float)maxSpeed);
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
            //position = getNextPosition(gameTime.ElapsedGameTime);

        }
    }
}