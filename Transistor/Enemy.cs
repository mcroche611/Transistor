using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class Enemy: Character
    {
        protected int damage;

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
                    // Me acerco por Y (pq está más cerca)
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
                    // Me acerco por X (pq está más cerca)
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

        public override void Move(TurnMode mode)
        {
            if (mode == TurnMode.Normal && field.Red.Pos != Pos)
            {
                base.Move(mode);
            }
        }

        public override bool Next(out Coor newPos)
        {
            bool possible = base.Next(out newPos);

            if (possible)
            {
                possible = !(field.Red.Pos == newPos) && !field.EnemyList.IsEnemy(newPos); //TOCHECK: && !field.ProjectileList.IsProjectile(newPos);
            }

            return possible;
        }
    }

    class Creep : Enemy
    {
        public Creep(Battlefield field, Coor pos) : base(field, pos)
        {
            life = 50;
            damage = 10;
            Symbols = "^^";
            BgColor = ConsoleColor.Yellow;
            Speed = 2; // igual a Player
        }

        
        public override void Move(TurnMode mode)
        {
            if (coolDown <= 0)
            {
                if (!Pos.Aligned(field.Red.Pos)) //si no está ya en línea con el jugador
                {
                    base.Move(mode); //note-to-self: rn they are dumb and run and will shoot straight into walls.
                }
                else if (mode != TurnMode.Normal)
                {
                    // que no se mueva
                }
                else
                {
                    //TODO: que no se mueva y al tiempo que ataque al jugador (y que aunque se mueva el Jugador, no vuelva a moverse hasta pasado un tiempo)
                }
            }
            else
            {
                coolDown--;
            }
        }

        public override void Attack(TurnMode mode, char attackMode)
        {
            if (coolDown <= 0)
            {
                if (field.Red.Pos.col == Pos.col) //misma columna
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

                    coolDown = 2;
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

                    coolDown = 2;
                    field.ProjectileList.Append(laser);
                }
            }
            else
            {
                coolDown--;
            }
        }
    }

    class Snapshot : Enemy
    {
        int minDistance = 5;
        Coor dirPred;

        public Snapshot(Battlefield field, Coor pos) : base(field, pos)
        {
            life = 50;
            damage = 10;
            Symbols = "##";
            BgColor = ConsoleColor.Green;
            coolDown = 20;
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

        public override void Move(TurnMode mode)
        {
            if (coolDown <= 0)
            {
                dirPred = field.Red.Dir;
                coolDown = 5;
            }
            else
            {
                coolDown--;
            }

            base.Move(mode);
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

                    coolDown = 20;
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

                    coolDown = 20;
                    field.ProjectileList.Append(shot);
                }
            }
        }
    }

    class Jerk : Enemy
    {
        public Jerk(Battlefield field, Coor pos) : base(field, pos)
        {
            life = 50;
            damage = 10;
            Symbols = "&&";
            BgColor = ConsoleColor.DarkCyan; 
            FgColor = ConsoleColor.Black;
            Speed = 4; // mitad que Player
        }

        // Move() básico

        //private int Range(int maxRange, Coor dir)
        //{
        //    int newRange = 0;
        //    bool outOfBoard = false;

        //    while (newRange <= maxRange && !outOfBoard)
        //    {
        //        if (NextDir(dir, Pos + new Coor(dir.row * newRange, dir.col * newRange)))
        //        {
        //            newRange++;
        //        }
        //        else
        //        {
        //            outOfBoard = true;
        //        }
        //    }

        //    return newRange;
        //}

        //private bool NextDir(Coor dir, Coor pos)
        //{
        //    Coor newPos = pos + dir;

        //    bool possible = field.tile[newPos.row, newPos.col] != Battlefield.Tile.BorderWall;

        //    return possible;
        //}

        public override void Attack(TurnMode mode, char attackMode)
        {
            Coor newPos;

            int maxRange = 3;

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
                    else if (field.ProjectileList.IsProjectile(newPos) && field.ProjectileList.GetProjectileInPos(newPos) is Load) //TOCHECK: Como todos lo tienen pero solo lo usa Load, hace falta validarlo?
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

    class Fetch : Enemy
    {
        public Fetch(Battlefield field, Coor pos) : base(field, pos)
        {
            life = 50;
            damage = 15;
            coolDown = 0;
            Symbols = "!!";
            BgColor = ConsoleColor.Red;
            FgColor = ConsoleColor.Black;
            Speed = 1; // mitad que Player
        }

        // Move() básico

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
                        coolDown = 3;
                    }
                    else if (field.ProjectileList.IsProjectile(newPos) && field.ProjectileList.GetProjectileInPos(newPos) is Load) //TOCHECK: Como todos lo tienen pero solo lo usa Load, hace falta validarlo?
                    {
                        Projectile p = field.ProjectileList.GetProjectileInPos(newPos);

                        if (p is Load)
                        {
                            p.ReceiveDamage();
                        }
                    }
                }
                else if (coolDown > 0)
                {
                    coolDown--;
                }
            }
        }
    }
}
