using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class Character 
    {
        protected Coor dir;
        protected Coor pos;
        protected string symbols;
        protected ConsoleColor bgColor;
        protected ConsoleColor fgColor;
        protected int life; 
        protected bool destroyed;
        protected int speed;
        protected Battlefield field;
        bool posChanged = true;
        

        public virtual Coor Pos
        {
            get => pos;
            set
            {
                pos = value;
                posChanged = true;
            }
        }

        public virtual Coor Dir
        {
            get => dir;
            set
            {
                dir = value;
                posChanged = true;
            }
        }

        public virtual string Symbols
        {
            get => symbols; 
            set => symbols = value;
        }

        public ConsoleColor BgColor
        {
            get => bgColor;
            set => bgColor = value;
        }

        public ConsoleColor FgColor
        {
            get => fgColor;
            set => fgColor = value;
        }

        public int Speed
        {
            get => speed; 
            set => speed = value;
        }

        public Character(Battlefield field, Coor pos)
        {
            this.field = field;
            Pos = pos;
            this.dir = Coor.ZERO;
        }

        public virtual bool Next(out Coor newPos)
        {
            newPos = Pos + Dir;

            bool possible = field.tile[newPos.row, newPos.col] == Battlefield.Tile.Empty;

            return possible;
        }

        public virtual void Move(TurnMode mode = TurnMode.Normal)
        {
            if (Next(out Coor newPos))
            {
                Pos = newPos;
            }
        }

        public void Painted()
        {
            posChanged = false;
        }

        public bool PosChanged
        {
            get => posChanged;
        }

        public bool Destroyed
        {
            get => destroyed;
        }

        public int Life
        {
            get => life;
            set => life = value;
        }
        
        public virtual void Attack(TurnMode mode, char attackMode)
        {

        }

        public virtual void ReceiveDamage(int damage)
        {
            field.Fx.PlayDamage();

            life -= damage; //resta el daño recibido a su vida

            if (life <= 0)
            {
                destroyed = true;
            }
        }
    }
}
