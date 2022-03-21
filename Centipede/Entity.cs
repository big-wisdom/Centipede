using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using static Centipede.GameView;

namespace Centipede
{
    public abstract class Entity
    {
        public Vector2 position;
        private Vector2 renderOffset;
        int radius;
        public CharachterEnum type;
        int maxSpeed;
        protected Vector2 velocity;

        public Entity(Vector2 position, Vector2 renderOffset, int radius, int maxSpeed, CharachterEnum type)
        {
            this.position = position;
            this.radius = radius;
            this.maxSpeed = maxSpeed;
            this.renderOffset = renderOffset;
        }

        public Vector2 renderPosition 
        { 
            get {
                return position + renderOffset;
            }
        }

        public abstract Rectangle computeSourceRectangle();

        public void move(double angle)
        {
            // set velocity
            float dx = (float)(maxSpeed * Math.Cos(angle));
            float dy = (float)(maxSpeed * Math.Sin(angle));
            velocity = new Vector2(dx, dy);
        }

        public abstract void update(GameTime gameTime, List<Collision> collisions);

        public void stop()
        {
            velocity = Vector2.Zero;
        }

        protected Vector2 getNextPosition(GameTime time) {
            return position + Vector2.Multiply(velocity, (float)time.ElapsedGameTime.TotalSeconds);
        }

        public List<Collision> checkBoundaryCollision(Rectangle rec, GameTime time) {

            Vector2 hypotheticalPosition = getNextPosition(time);
            List<Collision> collisions = new List<Collision>();

            if ((hypotheticalPosition.X - rec.X) < radius) {  // left wall
                collisions.Add(new Collision(CharachterEnum.leftWall, rec.X - ((int)position.X-radius)));
            }
            if (((rec.X+rec.Width) - hypotheticalPosition.X) < radius) {  // right wall
                collisions.Add(new Collision(CharachterEnum.rightWall, (rec.X+rec.Width)-((int)position.X+radius)));
            }
            if ((hypotheticalPosition.Y - rec.Y) < radius) {  // top wall
                collisions.Add(new Collision(CharachterEnum.topWall, ((int)position.Y-radius)-rec.Y));
            }
            if (((rec.Y+rec.Height) - hypotheticalPosition.Y) < radius) {  // bottom wall
                collisions.Add(new Collision(CharachterEnum.bottomWall, (rec.Y+rec.Height)-((int)position.Y+radius)));
            }

            return collisions;
        }
    }
}
