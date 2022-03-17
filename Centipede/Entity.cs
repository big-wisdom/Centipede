using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Centipede.GameView;

namespace Centipede
{
    public abstract class Entity
    {
        public Vector2 position;
        int radius;
        public CharachterEnum type;
        int maxSpeed;
        private Vector2 velocity;

        public Entity(Vector2 position, int radius, int maxSpeed, CharachterEnum type)
        {
            this.position = position;
            this.radius = radius;
            this.maxSpeed = maxSpeed;
        }

        public void move(double angle)
        {
            // set velocity
            float dx = (float)(maxSpeed * Math.Cos(angle));
            float dy = (float)(maxSpeed * Math.Sin(angle));
            velocity = new Vector2(dx, dy);
        }

        public void update(GameTime gametime)
        {
            Vector2 displacement = Vector2.Multiply(velocity, (float)(gametime.ElapsedGameTime.TotalSeconds));
            position += displacement;
        }

        public void stop()
        {
            velocity = Vector2.Zero;
        }
    }
}
