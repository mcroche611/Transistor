using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class Character 
    {
        public Coor pos, dir;
        protected int life;
        protected int coolDown;
        protected Battlefield field;

        public Character(Battlefield field, Coor pos)
        {
            this.field = field;
            this.pos = pos;
        }

        bool Next(Coor dir, out Coor newPos)
        {
            throw new NotImplementedException();            
        }

        void Move(TurnMode mode, Coor dir)
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
