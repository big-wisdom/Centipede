using System;
using System.Collections.Generic;
using System.Text;

namespace Centipede
{
    public abstract class IOverlay
    {
        //public abstract Enum OverlayOptions
        //{
        //    get;
        //}


        //int options = Enum.GetNames(typeof(OverlayOptions)).Length;

        //public OverlayOptions selected = OverlayOptions.Continue;

        public abstract void up();
        public abstract void down();

        protected abstract void constrainIndex();
        //{
        //    if (index >= options) index = 0;
        //    if (index < 0) index = options - 1;
        //    selected = (OverlayOptions)index;
        //}
    }
}
