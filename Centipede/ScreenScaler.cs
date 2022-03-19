using Microsoft.Xna.Framework.Graphics;

namespace Centipede
{
    public class ScreenScaler
    {
        public float screenScaleRatio;

        public ScreenScaler(GraphicsDevice gd)
        {
            screenScaleRatio = (float)gd.Viewport.Bounds.Width / (float)1280;
        }
    }
}
