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
        public int maxSpeed;
        protected Vector2 velocity;
        public abstract bool flip { get; }
        public double angle;

        public Entity(Vector2 position, Vector2 renderOffset, int radius, int maxSpeed, CharachterEnum type)
        {
            this.position = position;
            this.radius = radius;
            this.maxSpeed = maxSpeed;
            this.renderOffset = renderOffset;
            this.type = type;
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
            this.angle = angle;
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

        protected Vector2 getNextPosition(TimeSpan time) {
            return position + Vector2.Multiply(velocity, (float)time.TotalSeconds);
        }

        public List<Collision> checkBoundaryCollision(Rectangle rec, GameTime time) {

            Vector2 hypotheticalPosition = getNextPosition(time.ElapsedGameTime);
            List<Collision> collisions = new List<Collision>();

            if ((hypotheticalPosition.X - rec.X) < radius) {  // left wall
                collisions.Add(new Collision(CharachterEnum.leftWall, new Vector2(rec.X - (position.X - radius), 0)));
            }
            if (((rec.X+rec.Width) - hypotheticalPosition.X) < radius) {  // right wall
                collisions.Add(new Collision(CharachterEnum.rightWall, new Vector2((rec.X+rec.Width)-(position.X+radius), 0)));
            }
            if ((hypotheticalPosition.Y - rec.Y) < radius) {  // top wall
                collisions.Add(new Collision(CharachterEnum.topWall, new Vector2(0, rec.Y - (position.Y-radius))));
            }
            if (((rec.Y+rec.Height) - hypotheticalPosition.Y) < radius) {  // bottom wall
                collisions.Add(new Collision(CharachterEnum.bottomWall, new Vector2(0, (rec.Y+rec.Height)-(position.Y+radius))));
            }

            return collisions;
        }

        public Collision checkForCollision(Entity e, TimeSpan time)
        {
            Vector2 futureToCenter = e.getNextPosition(time) - getNextPosition(time);
            if (futureToCenter.Length() < (radius + e.radius))
            {
                Vector2 toCenter = e.position - position;
                Vector2 toImpact = Vector2.Multiply(Vector2.Normalize(toCenter), toCenter.Length() - (radius + e.radius));
                return new Collision(e.type, toImpact);
            }
                
            return null;
        }
    }
}
