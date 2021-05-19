using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class Character 
    {
        Coor pos, dir;
        int life;
        int coolDown;

        Battlefield field;

        public Character(Battlefield field, Coor pos)
        {
            this.field = field;
            this.pos = pos;
        }

        bool Next(Coor dir, out Coor newPos)
        {

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
