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
        protected int coolDown;
        protected Battlefield field;
        

        public virtual Coor Pos
        {
            get => pos; 
            set => pos = value;
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

        public Character(Battlefield field, Coor pos)
        {
            this.field = field;
            Pos = pos;
        }

        public virtual bool Next(out Coor newPos)
        {
            newPos = Pos + Dir;

            bool possible = field.tile[newPos.fil, newPos.col] == Battlefield.Tile.Empty;

            if (possible)
            {
                possible = !field.enemyList.IsEnemy(newPos) && !field.projectileList.IsProjectile(newPos);
            }

            return possible;
        }

        public virtual void Move(TurnMode mode)
        {
            
        }

        void Attack(TurnMode mode, char attackMode)
        {

        }

        void ReceiveDamage(int damage)
        {

        }
    }
}
