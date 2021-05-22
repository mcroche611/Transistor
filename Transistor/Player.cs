using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class Player: Character
    { 
        public override string Symbols
        {
            get
            {
                if (Dir.col == 0 && Dir.fil == 1)
                {
                    return ("@→");
                }
                else if (Dir.col == 0 && Dir.fil == -1)
                {
                    return ("←@");
                }
                else if (Dir.col == 1 && Dir.fil == 0)
                {
                    return ("@↑");
                }
                else if (Dir.col == -1 && Dir.fil == 0)
                {
                    return ("@↓");
                }
                else
                {
                    return ("@@");
                }
            }
            set => symbols = value;
        }

        public Player(Battlefield field, Coor pos) : base(field, pos)
        {
            Color = ConsoleColor.DarkRed;
        }

        
    }
}
