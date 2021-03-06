using System;
namespace Centipede
{
    public class Overlay : IOverlay
    {
        public enum OverlayOptions { 
            Continue,
            Quit
        }

        int options = Enum.GetNames(typeof(OverlayOptions)).Length;

        public OverlayOptions selected = OverlayOptions.Continue;

        private int index = 0;
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
            index -= 1;
            constrainIndex();
        }
    }
}
