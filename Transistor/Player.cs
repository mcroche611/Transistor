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
                if (Dir.col == 1 && Dir.row == 0)
                {
                    return ("@>");
                }
                else if (Dir.col == -1 && Dir.row == 0)
                {
                    return ("<@");
                }
                else if (Dir.col == 0 && Dir.row == -1)
                {
                    return ("@↑");
                }
                else if (Dir.col == 0 && Dir.row == 1)
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
            Speed = 2;
        }

        public override bool Next(out Coor newPos)
        {
            bool possible = base.Next(out newPos); 

            if (possible)
            {
                possible = !field.EnemyList.IsEnemy(newPos);
            }

            return possible;
        }

        public override void Move(TurnMode mode)
        {
            base.Move(mode);
        }
    }
}
