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
    }

    class Creep: Enemy
    {
        public Creep(Battlefield field, Coor pos) : base(field, pos)
        {
            Symbols = "^^";
            Color = ConsoleColor.Yellow;
        }

    }

    class Snapshot : Enemy
    {
        public Snapshot(Battlefield field, Coor pos) : base(field, pos)
        {
            Symbols = "##";
            Color = ConsoleColor.Green;
        }
    }

    class Jerk : Enemy
    {
        public Jerk(Battlefield field, Coor pos) : base(field, pos)
        {
            Symbols = "&&";
            Color = ConsoleColor.DarkCyan;
        }
    }

    class Fetch : Enemy
    {
        public Fetch(Battlefield field, Coor pos) : base(field, pos)
        {
            Symbols = "!!";
            Color = ConsoleColor.Red;
        }
    }
}
