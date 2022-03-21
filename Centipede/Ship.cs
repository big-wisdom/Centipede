using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static Centipede.GameView;

namespace Centipede
{
    public class Ship : Entity
    {
        public Ship(Vector2 position) : base(position, new Vector2(-14, -16), 20, 300, CharachterEnum.Ship)
        {
        }

        public override bool flip => false;

        public override Rectangle computeSourceRectangle()
        {
            return new Rectangle(0, 320, 28, 32);
        }

        public override void update(GameTime gametime, List<Collision> collisions)
        {
            if (collisions.Count != 0) {
                foreach (Collision collision in collisions) {
                    // check who the collision is with
                    CharachterEnum whoWith = collision.entityType;
                    // react accordingly
                    switch (whoWith) {
                        case CharachterEnum.topWall:
                            goto case CharachterEnum.bottomWall;
                        case CharachterEnum.bottomWall:
                            velocity = new Vector2(Vector2.Dot(velocity, Vector2.UnitX), 0);
                            break;

                        case CharachterEnum.leftWall:
                            goto case CharachterEnum.rightWall;
                        case CharachterEnum.rightWall:
                            velocity = new Vector2(0, Vector2.Dot(velocity, Vector2.UnitY));
                            break;
                    }
                }
            }
            position = getNextPosition(gametime.ElapsedGameTime);
        }
    }
}
