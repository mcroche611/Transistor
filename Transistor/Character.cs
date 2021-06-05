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
        protected int life; //TODO: maxLife
        protected bool destroyed;
        private bool hit;
        protected int speed;
        protected int coolDown;
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
            set => dir = value;
        }

        public virtual string Symbols
        {
            get => symbols; 
            set => symbols = value;
        }

        public ConsoleColor BgColor
        {
            get
            {
                ConsoleColor color;

                if (hit)
                {
                    color = ConsoleColor.White;
                    hit = false;
                }
                else
                {
                    color = bgColor;
                }

                return color;
            }

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

        public virtual void Move(TurnMode mode)
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
            get => posChanged ;
        }

        public bool Destroyed
        {
            get => destroyed;
        }

        public bool Hit 
        { 
            get => hit; 
            set => hit = value;
        }
        public int Life
        {
            get => life;
        }
        
        public virtual void Attack(TurnMode mode, char attackMode)
        {

        }

        public virtual void ReceiveDamage(int damage)
        {
            life -= damage;
            //hit = true;
            field.Fx.PlayShot1();

            if (life <= 0)
            {
                destroyed = true;
            }
        }
    }
}
