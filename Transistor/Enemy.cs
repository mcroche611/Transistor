using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class Enemy: Character
    {
        protected int damage;
        protected int coolDown = 0;
        protected int maxCoolDown;

        public Enemy(Battlefield field, Coor pos):base(field, pos)
        {
            
        }

        public override Coor Dir
        {
            get
            {
                Coor realDir;

                //si está más lejos del jugador por x que por y, y cuando el enemigo no esté en la misma fila, o si está, independientemente de la distancia, en la misma columna.
                if ((Pos.X_Distance(field.Red.Pos) > Pos.Y_Distance(field.Red.Pos) && field.Red.Pos.row != Pos.row) || field.Red.Pos.col == Pos.col) 
                {
                    // Se acerca por Y (porque está más cerca)
                    if (field.Red.Pos.row > Pos.row)
                    {
                        realDir = Coor.DOWN;
                    }
                    else
                    {
                        realDir = Coor.UP;
                    }
                }
                else
                {
                    // Se acerca por X (porque está más cerca)
                    if (field.Red.Pos.col > Pos.col)
                    {
                        realDir = Coor.RIGHT;
                    }
                    else
                    {
                        realDir = Coor.LEFT;
                    }
                }

                return realDir;
            }
            set => dir = value;
        }

        public override void Move(TurnMode mode = TurnMode.Normal)
        {
            if (field.Red.Pos != Pos)
            {
                base.Move();
            }
        }

        public override bool Next(out Coor newPos)
        {
            bool possible = base.Next(out newPos);

            if (possible)
            {
                possible = !(field.Red.Pos == newPos) && !field.EnemyList.IsEnemy(newPos); 
            }

            return possible;
        }
    }

    class Creep : Enemy
    {
        public Creep(Battlefield field, Coor pos) : base(field, pos)
        {
            life = 200;
            damage = 10;
            maxCoolDown = 6;
            Symbols = "^^";
            BgColor = ConsoleColor.Yellow;
            FgColor = ConsoleColor.Black;
            Speed = 1; // doble de velocidad que Player
        }

        public override void Move(TurnMode mode = TurnMode.Normal)
        {
            if (coolDown <= 0) //No se mueve y ataca simultáneamente
            {
                if (!Pos.Aligned(field.Red.Pos)) //Si no está ya en línea con el Player
                {
                    base.Move(mode); 
                }
            }
            else
            {
                coolDown--;
            }
        }

        public override void Attack(TurnMode mode, char attackMode)
        {
            if (field.Red.Pos.col == Pos.col) //Si está en la misma columna que el Player
            {
                Laser laser;

                if (field.Red.Pos.row > Pos.row) //Creep por encima de Player
                {
                    laser = new Laser(field, Pos, Coor.DOWN, damage);
                }
                else
                {
                    laser = new Laser(field, Pos, Coor.UP, damage);
                }

                coolDown = maxCoolDown;
                field.ProjectileList.Append(laser);
            }
            else if (field.Red.Pos.row == Pos.row)
            {
                Projectile laser;

                if (field.Red.Pos.col < Pos.col) //Creep a la derecha de Player
                {
                    laser = new Laser(field, Pos, Coor.LEFT, damage);
                }
                else
                {
                    laser = new Laser(field, Pos, Coor.RIGHT, damage);
                }

                coolDown = maxCoolDown;
                field.ProjectileList.Append(laser);
            }
        }
    }

    class Snapshot : Enemy
    {
        int minDistance = 5;
        Coor dirPred;

        public Snapshot(Battlefield field, Coor pos) : base(field, pos)
        {
            life = 250;
            damage = 5;
            maxCoolDown = 3;
            Symbols = "##";
            BgColor = ConsoleColor.Green;
            FgColor = ConsoleColor.Black;
            Speed = 2; // igual a Player
            dirPred = Coor.ZERO;
        }

        public override Coor Dir 
        { 
            get
            {
                Coor realDir;

                if (Pos.X_Distance(field.Red.Pos) > Pos.Y_Distance(field.Red.Pos)) //si está más lejos del jugador por x que por y
                {
                    if (Pos.X_Distance(field.Red.Pos) > minDistance) //si está a suficiente distancia
                    {
                        if (Pos.Aligned(field.Red.Pos)) //si está en línea con el jugador
                        {
                            realDir = Coor.ZERO;
                        }
                        else
                        {
                            realDir = dirPred; //sigue en la dirección a la que cree que va el jugador
                        }
                    }
                    else //si la distancia mínima es menor que la distancia al jugador, se aleja
                    {
                        if (field.Red.Pos.col > Pos.col)
                        {
                            realDir = Coor.LEFT;
                        }
                        else
                        {
                            realDir = Coor.RIGHT;
                        }
                    }
                }
                else
                {
                    if (Pos.Y_Distance(field.Red.Pos) > minDistance)  //si está a suficiente distancia 
                    {
                        if (Pos.Aligned(field.Red.Pos)) //si está en línea con el jugador
                        {
                            realDir = Coor.ZERO;
                        }
                        else
                        {
                            realDir = dirPred; //sigue en la dirección a la que cree que va el jugador
                        }
                    }
                    else //si la distancia mínima es menor que la distancia al jugador, se aleja
                    {
                        if (field.Red.Pos.row > Pos.row) 
                        {
                            realDir = Coor.DOWN; 
                        }
                        else
                        {
                            realDir = Coor.UP;
                        }
                    }
                }

                return realDir;
            }
            set => dir = value; 
        }

        public override void Move(TurnMode mode = TurnMode.Normal)
        {
            dirPred = field.Red.Dir; //se asigna la dirección del jugador en este momento

            if (coolDown > 0) //reduce el coolDown
                coolDown--;

            base.Move();
        }

        public override void Attack(TurnMode mode, char attackMode)
        {
            if (coolDown <= 0)
            {
                if (field.Red.Pos.col == Pos.col) //misma columna
                {
                    Shot shot;

                    if (field.Red.Pos.row > Pos.row) //Snapshot por encima de Player
                    {
                        shot = new Shot(field, Pos, Coor.DOWN, damage);
                    }
                    else
                    {
                        shot = new Shot(field, Pos, Coor.UP, damage);
                    }

                    coolDown = maxCoolDown;
                    field.ProjectileList.Append(shot);
                }
                else if (field.Red.Pos.row == Pos.row)
                {
                    Shot shot;

                    if (field.Red.Pos.col > Pos.col) //Snapshot a la izquierda de Player
                    {
                        shot = new Shot(field, Pos, Coor.RIGHT, damage);
                        
                    }
                    else
                    {
                        shot = new Shot(field, Pos, Coor.LEFT, damage);
                    }

                    coolDown = maxCoolDown;
                    field.ProjectileList.Append(shot);
                }
            }
        }
    }

    class Jerk : Enemy
    {
        public Jerk(Battlefield field, Coor pos) : base(field, pos)
        {
            life = 600;
            damage = 5;
            maxCoolDown = 2;
            Symbols = "&&";
            BgColor = ConsoleColor.DarkCyan; 
            FgColor = ConsoleColor.Black;
            Speed = 4; // mitad de velocidad que Player
        }

        public override void Move(TurnMode mode = TurnMode.Normal)
        {
            if (coolDown > 0)
                coolDown--;

             base.Move(mode);
        }

        public override void Attack(TurnMode mode, char attackMode)
        {
            Coor newPos;

            int maxRange = 2; //Radio del área en el que hace daño al jugador

            if (coolDown <= 0)
            {
                //Chequeo del jugador en un área en rombo 
                for (int j = -maxRange; j <= maxRange; j++)
                {
                    int col;
                    if (j <= 0)
                        col = (maxRange + j);
                    else
                        col = (maxRange - j);

                    for (int k = -col; k <= col; k++)
                    {
                        newPos = Pos + new Coor(j, k);

                        if (newPos == field.Red.Pos)
                        {
                            field.Red.ReceiveDamage(damage);
                        }
                        else if (field.ProjectileList.IsProjectile(newPos) && field.ProjectileList.GetProjectileInPos(newPos) is Load) 
                        {
                            Projectile p = field.ProjectileList.GetProjectileInPos(newPos);

                            if (p is Load)
                            {
                                p.ReceiveDamage();
                            }
                        }
                    }
                }
            }
        }
    }

    class Fetch : Enemy
    {
        public Fetch(Battlefield field, Coor pos) : base(field, pos)
        {
            life = 580;
            damage = 10;
            maxCoolDown = 6;
            Symbols = "!!";
            BgColor = ConsoleColor.Red;
            FgColor = ConsoleColor.Black;
            Speed = 1; // doble de velocidad que Player
        }

        public override void Move(TurnMode mode = TurnMode.Normal)
        {
            if (coolDown <= 0) //No se mueve si acaba de atacar
            {
                base.Move();
            }
            else
            {
                coolDown--;
            }
        }

        public override void Attack(TurnMode mode, char attackMode)
        {
            Next(out Coor newPos); //obtenemos la siguiente posición

            if (field.Red.Pos == newPos) //Si Player está en la posición de ataque
            {
                if (coolDown <= 0) //coolDown transcurrido
                {
                    if (newPos == field.Red.Pos)
                    {
                        field.Red.ReceiveDamage(damage);
                        coolDown = maxCoolDown;  
                    }
                    else if (field.ProjectileList.IsProjectile(newPos) && field.ProjectileList.GetProjectileInPos(newPos) is Load)
                    {
                        Projectile p = field.ProjectileList.GetProjectileInPos(newPos);

                        if (p is Load)
                        {
                            p.ReceiveDamage();
                        }
                    }
                }
            }
        }
    }
}
