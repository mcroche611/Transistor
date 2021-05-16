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

        Battlefield field = new Battlefield("");
        Character(Battlefield field)
        {

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
