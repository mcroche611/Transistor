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
        private ConsoleColor color;
        protected int life;
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

        public ConsoleColor Color
        {
            get => color;
            set => color = value;
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

        public virtual void Attack(TurnMode mode, char attackMode)
        {

        }

        void ReceiveDamage(int damage)
        {

        }
    }
}
