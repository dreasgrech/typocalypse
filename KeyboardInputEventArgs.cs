using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Typocalypse
{
    class KeyboardInputEventArgs:EventArgs
    {
        public char Character{ get; set;}
        public KeyboardInputEventArgs(char character)
        {
            Character = character;
        }
    }
}
