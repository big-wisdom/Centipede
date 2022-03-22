using System;
using System.Collections.Generic;
using System.Text;

namespace Centipede
{
    class GameOverOverlay: IOverlay
    {
        public enum OverlayOptions
        {
            Restart,
            Menu,
        }

        int options = Enum.GetNames(typeof(OverlayOptions)).Length;
        int index = 0;

        public OverlayOptions selected = OverlayOptions.Restart;

        protected override void constrainIndex()
        {
            if (index >= options) index = 0;
            if (index < 0) index = options - 1;
            selected = (OverlayOptions)index;
        }

        public override void up()
        {
            index += 1;
            constrainIndex();
        }

        public override void down()
        {
            index += 1;
            constrainIndex();
        }
    }
}
