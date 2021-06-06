using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class Player: Character
    {
        protected string turnMoves;

        //posiciones en cada array de los atributos de cada ataque
        private const int crashNum = 0;
        private const int breachNum = 1;
        private const int pingNum = 2;
        private const int loadNum = 3;

        private int moveTurn = 2;
        private int[] attacksTurn = { 10, 30, 10, 20 }; //Coste de la barra de Turn de cada ataque
        private int[] attacksDamage = { 5, 15, 5, 30 };
        private bool[] attacksEnabled = { true, true, true, true };
        private int[] attacksMaxCoolDown = { 5, 30, 5, 40 }; //Cooldown de cada uno de los ataques del jugador
        private int[] attacksCoolDown = { 0, 0, 0, 0 }; //Cooldown de cada uno de los ataques del jugador

        Random rnd = new Random();
        private Coor posTurn; //La posición inicial al comienzo de una fase de planificación
        private TurnMode attackMode; //Guarda el modo en el que se realizan los ataques para disminuir o no la barra de Turn

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

        public bool[] AttacksEnabled
        {
            get => attacksEnabled;
        }

        public int[] AttacksMaxCoolDown
        {
            get => attacksMaxCoolDown;
        }

        public int[] AttacksCoolDown
        {
            get => attacksCoolDown;
            set => attacksCoolDown = value;
        }

        public Player(Battlefield field, Coor pos) : base(field, pos)
        {
            life = 100;
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

            if (mode == TurnMode.Plan) //Guarda el movimiento a ejecutar durante Run
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
            }

            if (mode != TurnMode.Normal) //Resta coste del movimiento a Turn
            {
                field.TurnPercentage -= moveTurn;
            }
        }

        public override void Attack(TurnMode mode, char attack)
        {
            attackMode = mode;

            switch (attack)
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

            if (mode == TurnMode.Plan) //Guarda el ataque a ejecutar durante Run si el juego está en mode Plan
            {
                turnMoves += attack;
            }
        }

        private void Crash()
        {
            if (attackMode == TurnMode.Run || (attackMode == TurnMode.Normal && attacksCoolDown[crashNum] <= 0)) //En Run no importa el coolDown, pero en Normal solo ataca si el coolDown ha llegado a 0
            {
                Next(out Coor newPos);

                Enemy e = field.EnemyList.GetEnemyInPos(newPos);

                if (e != null)
                {
                    e.ReceiveDamage(attacksDamage[crashNum]);
                }
                else if (field.ProjectileList.IsProjectile(newPos) && field.ProjectileList.GetProjectileInPos(newPos) is Load) //TOCHECK: Como todos lo tienen pero solo lo usa Load, hace falta validarlo?
                {
                    Projectile p = field.ProjectileList.GetProjectileInPos(newPos);

                    if (p is Load)
                    {
                        p.ReceiveDamage();
                    }
                }

                if (attackMode == TurnMode.Normal)
                    attacksCoolDown[crashNum] = attacksMaxCoolDown[crashNum]; //Si el ataque es en modo normal, resetear el coolDown
            }

            if (attackMode != TurnMode.Normal)
                field.TurnPercentage -= attacksTurn[crashNum];
        }

        private void Breach()
        {
            if (attackMode == TurnMode.Run || (attackMode == TurnMode.Normal && attacksCoolDown[breachNum] <= 0))
            {
                Beam beam = new Beam(field, Pos, Dir, attacksDamage[breachNum]);

                field.ProjectileList.Append(beam);

                if (attackMode == TurnMode.Normal)
                    attacksCoolDown[breachNum] = attacksMaxCoolDown[breachNum]; //Si el ataque es en modo normal, resetear el coolDown
            }

            if (attackMode != TurnMode.Normal)
                field.TurnPercentage -= attacksTurn[breachNum];
        }

        private void Ping()
        {
            if (attackMode == TurnMode.Run || (attackMode == TurnMode.Normal && attacksCoolDown[pingNum] <= 0))
            {
                Bullet bullet = new Bullet(field, Pos, Dir, attacksDamage[pingNum]);

                field.ProjectileList.Append(bullet);

                if (attackMode == TurnMode.Normal)
                    attacksCoolDown[pingNum] = attacksMaxCoolDown[pingNum]; //Si el ataque es en modo normal, resetear el coolDown
            }

            if (attackMode != TurnMode.Normal)
                field.TurnPercentage -= attacksTurn[pingNum];
        }

        private void Load()
        {
            if (attackMode == TurnMode.Run || (attackMode == TurnMode.Normal && attacksCoolDown[loadNum] <= 0))
            {
                if (Next(out Coor newPos)) //Solo si se puede colocar sobre la próxima posición
                {
                    Load load = new Load(field, newPos, Dir, attacksDamage[loadNum]);

                    field.ProjectileList.Append(load);

                    if (attackMode == TurnMode.Normal)
                        attacksCoolDown[loadNum] = attacksMaxCoolDown[loadNum]; //Si el ataque es en modo normal, resetear el coolDown
                }
            }

            if (attackMode != TurnMode.Normal)
                field.TurnPercentage -= attacksTurn[loadNum];
        }

        public char GetActionTurn()
        {
            char c = '\0'; 

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

        public override void ReceiveDamage(int damage)
        {
            life -= damage;

            if (life <= 0)
            {
                //Deshabilitar un ataque random de cuatro
                int attackRemoved;
                int numAttacks;
                do
                {
                    attackRemoved = rnd.Next(0, 4);

                    //Cuenta el número de ataques que le quedan disponibles al jugador
                    numAttacks = CountAttacks();
                }
                while (attacksEnabled[attackRemoved] == false && numAttacks > 0);

                attacksEnabled[attackRemoved] = false;

                numAttacks = CountAttacks();

                //Si todavía le quedan ataques habilitados al jugador, recupera la vida
                if (numAttacks != 0)
                {
                    life = 100;
                }
                else
                {
                    life = 0;
                }
            }
        }

        private int CountAttacks()
        {
            int numAttacks = 0;
            for (int i = 0; i < attacksEnabled.Length; i++)
            {
                if (attacksEnabled[i])
                    numAttacks++;
            }

            return numAttacks;
        }
    }
}
