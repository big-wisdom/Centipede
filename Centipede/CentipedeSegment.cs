using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using static Centipede.GameView;

namespace Centipede
{
    public class CentipedeSegment: Entity
    {
        Rectangle bigWalls;
        double previousAngle;

        int row = 1;
        public Rectangle walls { 
            get {
                return new Rectangle(0, 0, bigWalls.Width, row * 32);
            }
        }
        
        public CentipedeSegment(Vector2 position, Vector2 offset, double angle, int startingFrame, Rectangle walls): base(position, offset, 16, 300, CharachterEnum.Centipede)
        {
            previousAngle = angle;
            move(angle);
            this.bigWalls = walls;
        }

        public override Rectangle computeSourceRectangle()
        {
            return new Rectangle(0, 64, 32, 32);
        }

        public override void update(GameTime gameTime, List<Collision> collisions)
        {
            foreach (Collision c in collisions) {
                switch (c.entityType) {
                    case CharachterEnum.rightWall:
                        goto case CharachterEnum.leftWall;
                    case CharachterEnum.leftWall:
                        row++;
                        move(Math.PI / 2);
                        // snap into grid
                        position += new Vector2(c.distance, 0);
                        break;

                    case CharachterEnum.bottomWall:
                        previousAngle = (previousAngle + Math.PI) % (2 * Math.PI);
                        move(previousAngle);
                        // position += new Vector2(0, c.distance);
                        break;
                }
            }
            position = getNextPosition(gameTime);
        }
    }
}
