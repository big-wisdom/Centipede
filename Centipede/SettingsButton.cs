using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static Centipede.GameView;

namespace Centipede
{
    class SettingsButton
    {
        Rectangle rec;
        public ControlsEnum control;
        public bool back;

        public SettingsButton(Rectangle rec, ControlsEnum control, bool backButton)
        {
            this.rec = rec;
            this.control = control;
            this.back = backButton;
        }

        public bool clicked(int x, int y)
        {
            if (rec.Contains(x, y))
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
