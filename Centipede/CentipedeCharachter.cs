using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Centipede
{
    public class CentipedeCharachter
    {
        public List<CentipedeSegment> centipede = new List<CentipedeSegment>();

        public CentipedeCharachter(Vector2 position, Vector2 offset, int cellSize, Rectangle walls)
        {
            // randomize direction
            Random rnd = new Random();
            int direction = rnd.Next(2); // 0 means left 1 means right
            double angle;
            if (direction == 0) {
                angle = Math.PI;
            } else {
                angle = 0;
            }

            for (int i=0; i<1; i++) {
                // calculate position of each
                Vector2 interval = Vector2.Multiply(new Vector2((int)(Math.Cos(angle) * cellSize), 0), i);
                centipede.Add(new CentipedeSegment(position+interval, offset, angle, i%8, walls));
            }
        }

        public List<CentipedeSegment> removeDeadSegments()
        {
            List<CentipedeSegment> removeList = new List<CentipedeSegment>();
            foreach (CentipedeSegment s in centipede)
            {
                if (!s.alive)
                    removeList.Add(s);
            }
            
            foreach (CentipedeSegment s in removeList)
            {
                centipede.Remove(s);
            }

            return removeList;
        }
    }
}
