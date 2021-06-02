using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class Enemy: Character
    {
        public Enemy(Battlefield field, Coor pos):base(field, pos)
        {

        }

        public override Coor Dir
        {
            get
            {
                if (Math.Abs(field.Red.Pos.col - Pos.col) > Math.Abs(field.Red.Pos.row - Pos.row)) //si está más lejos del jugador por x que por y
                {
                    if (field.Red.Pos.row > Pos.row)
                    {
                        return Coor.DOWN;
                    }
                    else
                    {
                        return Coor.UP;
                    }
                }
                else
                {
                    if (field.Red.Pos.col > Pos.col)
                    {
                        return Coor.RIGHT;
                    }
                    else
                    {
                        return Coor.LEFT;             
                    }
                }
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
                possible = !(field.Red.Pos == newPos) && !field.EnemyList.IsEnemy(newPos); // && !field.ProjectileList.IsProjectile(newPos);
            }

            return possible;
        }
    }

    class Creep : Enemy
    {
        public Creep(Battlefield field, Coor pos) : base(field, pos)
        {
            Symbols = "^^";
            Color = ConsoleColor.Yellow;
            Speed = 2; // igual a Player
        }

        public override void Move(TurnMode mode)
        {
            if (field.Red.Pos.col != Pos.col && field.Red.Pos.row != Pos.row) //si no está ya en línea con el jugador
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

        public override void Attack(TurnMode mode, char attackMode)
        {
            if (coolDown <= 0)
            {
                if (field.Red.Pos.col == Pos.col) //misma columna
                {
                    Laser laser;

                    if (field.Red.Pos.col > Pos.col) //Creep a la izquierda de Player
                    {
                        laser = new Laser(field, Pos + Coor.LEFT);
                    }
                    else
                    {
                        laser = new Laser(field, Pos + Coor.RIGHT);
                    }

                    coolDown = 2;
                    field.ProjectileList.Append(laser);
                }
                else if (field.Red.Pos.row == Pos.row)
                {
                    Projectile laser;

                    if (field.Red.Pos.row > Pos.row) //Creep por encima de Player
                    {
                        laser = new Laser(field, Pos + Coor.DOWN);
                    }
                    else
                    {
                        laser = new Laser(field, Pos + Coor.UP);
                    }

                    coolDown = 2;
                    field.ProjectileList.Append(laser);
                }
            }
        }
    }

    class Snapshot : Enemy
    {
        int minDistance = 5;
        Coor dirPred;

        public Snapshot(Battlefield field, Coor pos) : base(field, pos)
        {
            Symbols = "##";
            Color = ConsoleColor.Green;
            coolDown = 50;
            Speed = 2; // igual a Player
            dirPred = Coor.ZERO;
        }

        public override Coor Dir 
        { 
            get
            {
                if (Math.Abs(field.Red.Pos.col - Pos.col) > Math.Abs(field.Red.Pos.row - Pos.row)) //si está más lejos del jugador por x que por y
                {
                    if ((Math.Abs(field.Red.Pos.col - Pos.col) > minDistance)) //si está a suficiente distancia
                    {
                        if (field.Red.Pos.col == Pos.col || field.Red.Pos.row == Pos.row) //si está en línea con el jugador
                        {
                            return Coor.ZERO;
                        }
                        else
                        {
                            return dirPred; //sigue en la dirección a la que cree que va el jugador
                        }
                    }
                    else //si la distancia mínima es menor que la distancia al jugador, se aleja
                    {
                        if (field.Red.Pos.col > Pos.col)
                        {
                            return Coor.RIGHT;
                        }
                        else
                        {
                            return Coor.LEFT;
                        }
                    }
                }
                else
                {
                    if ((Math.Abs(field.Red.Pos.row - Pos.row) > minDistance))  //si está a suficiente distancia 
                    {
                        if (field.Red.Pos.row == Pos.row || field.Red.Pos.col == Pos.col) //si está en línea con el jugador
                        {
                            return Coor.ZERO;
                        }
                        else
                        {
                            return dirPred; //sigue en la dirección a la que cree que va el jugador
                        }
                    }
                    else //si la distancia mínima es menor que la distancia al jugador, se aleja
                    {
                        if (field.Red.Pos.row > Pos.row) 
                        {
                            return Coor.UP; 
                        }
                        else
                        {
                            return Coor.DOWN;
                        }
                    }
                }
            }
            set => dir = value; 
        }

        public override void Move(TurnMode mode)
        {
            if (coolDown <= 0)
            {
                dirPred = field.Red.Dir;
                coolDown = 2;
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

                    if (field.Red.Pos.col > Pos.col) //Creep a la izquierda de Player
                    {
                        shot = new Shot(field, Pos + Coor.LEFT);
                    }
                    else
                    {
                        shot = new Shot(field, Pos + Coor.RIGHT);
                    }

                    coolDown = 2;
                    field.ProjectileList.Append(shot);
                }
                else if (field.Red.Pos.row == Pos.row)
                {
                    Shot shot;

                    if (field.Red.Pos.row > Pos.row) //Creep por encima de Player
                    {
                        shot = new Shot(field, Pos + Coor.DOWN);
                    }
                    else
                    {
                        shot = new Shot(field, Pos + Coor.UP);
                    }

                    coolDown = 2;
                    field.ProjectileList.Append(shot);
                }
            }
        }
    }

    class Jerk : Enemy
    {
        public Jerk(Battlefield field, Coor pos) : base(field, pos)
        {
            Symbols = "&&";
            Color = ConsoleColor.DarkCyan;
            Speed = 4; // doble que Player
        }

        // Move() básico

        public override void Attack(TurnMode mode, char attackMode)
        {
            Coor newPos = new Coor();

            int range = 2; //TODO: dónde se debería inicializar?
            int i = 0;

            //Chequeo del jugador en un área en rombo
            for (int j = -range; j <= range; j++)
            {
                for (int k = i; k <= Math.Abs(i); k++)
                {
                    newPos = Pos + new Coor(j, k);

                    if (newPos == field.Red.Pos)
                    {
                        //InflictDamage(); //TODO: implementar método
                    }

                    i--;
                }
            }
        }
    }

    class Fetch : Enemy
    {
        public Fetch(Battlefield field, Coor pos) : base(field, pos)
        {
            Symbols = "!!";
            Color = ConsoleColor.Red;
            Speed = 1; // mitad que Player
        }

        // Move() básico

        public override void Attack(TurnMode mode, char attackMode)
        {
            if (coolDown <= 0 && Next(out Coor newPos)) //coolDown transcurrido y obtenemos la siguiente posición
            {
                if (newPos == field.Red.Pos)
                {
                    //InflictDamage(); //TODO: crear el método
                    coolDown = 3;
                }
            }
            else if (coolDown > 0)
            {
                coolDown--;
            }
        }
    }
}
