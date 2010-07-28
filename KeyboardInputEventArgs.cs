using System;

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
