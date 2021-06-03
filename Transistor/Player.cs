using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class Player: Character
    {
        protected string turnMoves;

        public string TurnMoves
        {
            get => turnMoves;
        }

        public override string Symbols
        {
            get
            {
                string s;

                if (Dir.col == 1 && Dir.row == 0)
                {
                    s = ("@>");
                }
                else if (Dir.col == -1 && Dir.row == 0)
                {
                    s = ("<@");
                }
                else if (Dir.col == 0 && Dir.row == -1)
                {
                    s = ("@↑");
                }
                else if (Dir.col == 0 && Dir.row == 1)
                {
                    s = ("@↓");
                }
                else
                {
                    s = ("@@");
                }

                return s;
            }
            set => symbols = value;
        }

        public Player(Battlefield field, Coor pos) : base(field, pos)
        {
            life = 100;
            damage = 20;
            dir = Coor.RIGHT;
            BgColor = ConsoleColor.DarkRed;
            FgColor = ConsoleColor.White;
            Speed = 2;
            turnMoves = "";
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

            if (mode == TurnMode.Plan)
            {
                if (Dir.col == 1 && Dir.row == 0)
                {
                    turnMoves += ("r");
                }
                else if (Dir.col == -1 && Dir.row == 0)
                {
                    turnMoves += ("i");
                }
                else if (Dir.col == 0 && Dir.row == -1)
                {
                    turnMoves += ("u");
                }
                else if (Dir.col == 0 && Dir.row == 1)
                {
                    turnMoves += ("d");
                }
            }
        }

        public override void Attack(TurnMode mode, char attackMode)
        {
            if (mode == TurnMode.Normal)
            {
                Next(out Coor newPos);

                Enemy e = field.EnemyList.GetEnemyInPos(newPos);

                if (e != null)
                {
                    e.ReceiveDamage(damage);
                }
            }
            else if (mode == TurnMode.Plan)
            {
                turnMoves += attackMode;
            }
        }
    }
}
