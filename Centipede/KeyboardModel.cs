using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Centipede
{
    public class KeyboardModel
    {
        HashSet<Keys> locked = new HashSet<Keys>();

        public List<Keys> GetUnlockedKeys() {
            KeyboardState keyboard = Keyboard.GetState();
            List<Keys> unlockedKeys = new List<Keys>( keyboard.GetPressedKeys());

            // remove all locked keys
            List<Keys> unlockList = new List<Keys>();
            foreach (Keys k in locked) {
                if (!unlockedKeys.Remove(k)) {
                    unlockList.Add(k); // if key no longer pressed then unlock it
                }
            }
            foreach (Keys k in unlockList) locked.Remove(k); // here's the loop where it's actually unlocked

            return unlockedKeys;
        }

        public void lockKey(Keys key) {
            locked.Add(key);
        }

        public void unlockKey(Keys key) {
            locked.Remove(key);
        }
    }
}
