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
        bool hit = false;
        public bool dead = false;
        int frame = 0;

        TimeSpan animationMilliseconds = TimeSpan.FromMilliseconds(25);
        TimeSpan timeTillAnimate = TimeSpan.FromMilliseconds(25);

        public Ship(Vector2 position) : base(position, new Vector2(-14, -16), 20, 300, CharachterEnum.Ship)
        {
        }

        private void updateFrame(GameTime gameTime)
        {
            timeTillAnimate -= gameTime.ElapsedGameTime;
            if (timeTillAnimate.TotalMilliseconds < 0)
            {
                timeTillAnimate = animationMilliseconds + timeTillAnimate;
                frame++;
                if (frame > 6) dead = true;
            }
        }

        public override bool flip => false;

        public override Rectangle computeSourceRectangle()
        {
            if (!hit)
            {
                return new Rectangle(0, 320, 28, 32);
            } else
            {
                int x = 256;
                int y = 256;
                if (frame % 2 == 0)
                {
                    x += 32 * (frame/2);
                } else
                {
                    y += 32;
                    x += ((frame - 1) / 2) * 32;
                }
                return new Rectangle(x, y, 32, 32);
            }

        }

        public override void update(GameTime gametime, List<Collision> collisions)
        {
            if (!dead)
            {
                if (hit)
                {
                    updateFrame(gametime);
                }
                else
                {
                    if (collisions.Count != 0)
                    {
                        foreach (Collision collision in collisions)
                        {
                            // check who the collision is with
                            CharachterEnum whoWith = collision.entityType;
                            // react accordingly
                            switch (whoWith)
                            {
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

                                case CharachterEnum.Mushroom:
                                    velocity = Vector2.Zero;
                                    //collisionWithEntity(collision.displacement);
                                    break;

                                case CharachterEnum.Centipede:
                                    hit = true;
                                    break;
                            }
                        }
                    }
                    position = getNextPosition(gametime.ElapsedGameTime);
                }
            }
        }

        // this funtion projects the velocity along the tangent to the circle being impacted
        // it's cool but it causes a few bugs
        // if you're between a wall and mushroom, sometimes you can get the ship to just disappear.
        // I assume because the mushroom pushes it outside and for some reason that makes it disappear
        
        // also every once in a while you can go straight through a mushroom
        private void collisionWithEntity(Vector2 r)
        {
            Vector3 r3 = new Vector3(r, 0);
            Vector3 v3 = new Vector3(velocity, 0);
            Vector3 unitTangent3 = Vector3.Normalize(Vector3.Cross(r3, Vector3.Cross(v3, r3)));
            // turn back to Vector2
            Vector2 unitTangent = new Vector2(unitTangent3.X, unitTangent3.Y);
            velocity = Vector2.Multiply(unitTangent, velocity.Length());
        }
    }
}
