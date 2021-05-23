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
                    return ("@>");
                }
                else if (Dir.col == 0 && Dir.fil == -1)
                {
                    return ("<@");
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
            dir = new Coor(0, 1);
            Color = ConsoleColor.DarkRed;
        }

        public override void Move(TurnMode mode, Coor dir)
        {
            if (Next(out Coor newPos))
            {
                //if (field.tile[Pos.fil, Pos.col] == Battlefield.Tile.Empty) // isn't this already checked????
                {
                    Pos = newPos;
                }
            }
        }
    }
}
