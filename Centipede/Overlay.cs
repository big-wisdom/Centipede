using System;
namespace Centipede
{
    public class Overlay
    {
        public enum OverlayOptions { 
            Continue,
            Quit
        }

        int options = Enum.GetNames(typeof(OverlayOptions)).Length;

        private int index = 0;
        public OverlayOptions selected = OverlayOptions.Continue;

        public void up() {
            index += 1;
            constrainIndex();
        }

        public void down() {
            index -= 1;
            constrainIndex();
        }

        private void constrainIndex() { 
            if (index >= options) index = 0;
            if (index < 0) index = options-1;
            selected = (OverlayOptions)index;
        }
    }
}
