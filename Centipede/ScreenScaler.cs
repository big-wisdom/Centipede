using Microsoft.Xna.Framework.Graphics;

namespace Centipede
{
    public class ScreenScaler
    {
        public float screenScaleRatio;
        public string m_fontMenu;
        public string m_fontMenuSelect;
        public int cellSize;

        public ScreenScaler(GraphicsDevice gd)
        {
            screenScaleRatio = (float)gd.Viewport.Bounds.Width / (float)1280;

            if (screenScaleRatio < .66) {
                m_fontMenu = "Fonts/menu-small";
                m_fontMenuSelect = "Fonts/menu-select-small";
            } else {
                m_fontMenu = "Fonts/menu";
                m_fontMenuSelect = "Fonts/menu-select";
            }

            cellSize = gd.Viewport.Bounds.Width / 40; // number of cells in an average screen
        }
    }
}
