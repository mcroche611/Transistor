using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class Player: Character
    {
        protected string turnMoves;
        private int moveTurn = 2;
        private int crashTurn = 10;
        private int breachTurn = 10;
        private int pingTurn = 10;
        private int loadTurn = 10;
        private const int crashDamage = 5;
        private const int breachDamage = 15;
        private const int pingDamage = 5;
        private const int loadDamage = 30;
        private Coor posTurn;

        public string TurnMoves
        {
            get => turnMoves;
        }

        public override string Symbols
        {
            get
            {
                string s;

                if (Dir == Coor.RIGHT)
                {
                    s = ("@>");
                }
                else if (Dir == Coor.LEFT)
                {
                    s = ("<@");
                }
                else if (Dir == Coor.UP)
                {
                    s = ("@↑");
                }
                else if (Dir == Coor.DOWN)
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

        internal Coor PosTurn
        {
            get => posTurn;
            set => posTurn = value;
        }

        public Player(Battlefield field, Coor pos) : base(field, pos)
        {
            life = 100;
            dir = Coor.RIGHT;
            SetColor(ConsoleColor.DarkRed, ConsoleColor.White);
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
                if (Dir == Coor.RIGHT)
                {
                    turnMoves += ("r");
                }
                else if (Dir == Coor.LEFT)
                {
                    turnMoves += ("i");
                }
                else if (Dir == Coor.UP)
                {
                    turnMoves += ("u");
                }
                else if (Dir == Coor.DOWN)
                {
                    turnMoves += ("d");
                }

                field.TurnPercentage -= moveTurn;
            }
        }

        public override void Attack(TurnMode mode, char attack)
        {
            if (mode != TurnMode.Plan)
            {
                switch(attack)
                {
                    case 'c':
                        Crash();
                        break;
                    case 'b':
                        Breach();
                        break;
                    case 'p':
                        Ping();
                        break;
                    case 'l':
                        Load();
                        break;
                }
            }
            else
            {
                turnMoves += attack;
            }
        }

        private void Crash()
        {
            Next(out Coor newPos);

            Enemy e = field.EnemyList.GetEnemyInPos(newPos);

            if (e != null)
            {
                e.ReceiveDamage(crashDamage);
            }

            field.TurnPercentage -= crashTurn;
        }

        private void Breach()
        {
            Beam beam = new Beam(field, Pos, Dir, breachDamage);

            field.ProjectileList.Append(beam);

            field.TurnPercentage -= breachTurn;
        }

        private void Ping()
        {
            Bullet bullet = new Bullet(field, Pos, Dir, pingDamage);

            field.ProjectileList.Append(bullet);

            field.TurnPercentage -= pingTurn;
        }

        private void Load()
        {
            Load load = new Load(field, Pos, Dir, loadDamage);

            field.ProjectileList.Append(load);

            field.TurnPercentage -= loadTurn;
        }

        public char GetActionTurn()
        {
            char c = '\0'; //TODO: Comprobaciones de c != ' ' a c != '\0'

            if (turnMoves.Length > 0)
            {
                c = turnMoves[0];

                if (turnMoves.Length > 1)
                {
                    turnMoves = turnMoves.Substring(1);
                }
                else
                {
                    turnMoves = "";
                }
            }

            return c;
        }
    }
}
