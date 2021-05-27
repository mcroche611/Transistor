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
                    if (field.Red.Pos.col > Pos.col)
                    {
                        return new Coor(-1, 0); //left
                    }
                    else
                    {
                        return new Coor(1, 0); //right
                    }
                }
                else
                {
                    if (field.Red.Pos.row > Pos.row)
                    {
                        return new Coor(0, -1); //up
                    }
                    else
                    {
                        return new Coor(0, 1); //down
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
        }

        public override Coor Dir 
        { 
            get
            {
                if (Math.Abs(field.Red.Pos.col - Pos.col) > Math.Abs(field.Red.Pos.row - Pos.row)) //si está más lejos del jugador por x que por y
                {
                    if (Math.Abs(field.Red.Pos.col - Pos.col) > minDistance)
                    {
                        return dirPred; //sigue en la dirección a la que cree que va el jugador
                    }
                    else
                    {
                        if (field.Red.Pos.col > Pos.col) //si la distancia mínima es menor que la distancia al jugador, se aleja
                        {
                            return new Coor(-1, 0); //left
                        }
                        else
                        {
                            return new Coor(1, 0); //right
                        }
                    }
                }
                else
                {
                    if (Math.Abs(field.Red.Pos.row - Pos.row) > minDistance)
                    {
                        return dirPred; //sigue en la dirección a la que cree que va el jugador
                    }
                    else
                    {
                        if (field.Red.Pos.col > Pos.col) //si la distancia mínima es menor que la distancia al jugador, se aleja
                        {
                            return new Coor(0, 1); //down
                        }
                        else
                        {
                            return new Coor(0, -1); //up
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
                coolDown = 50;
            }
            else
            {
                coolDown -= 10;
            }

            base.Move(mode);
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
    }
}
