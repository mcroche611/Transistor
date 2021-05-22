using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class Projectile
    {
        private Coor dir;
        private Coor pos;
        private string symbols;
        private ConsoleColor fgColor;
        private ConsoleColor bgColor;
        protected Battlefield field;
        

        public Projectile(Battlefield field, Coor pos)
        {
            this.field = field;
            Pos = pos;
        }

        internal Coor Pos
        {
            get => pos;
            set => pos = value;
        }

        internal Coor Dir
        {
            get => dir;
            set => dir = value;
        }

        public string Symbols
        {
            get => symbols;
            set => symbols = value;
        }

        public ConsoleColor FgColor
        {
            get => fgColor;
            set => fgColor = value;
        }

        public ConsoleColor BgColor
        {
            get => bgColor;
            set => bgColor = value;
        }

        public void Move()
        {
            Pos += Dir;
        }
    }

    class Laser: Projectile
    {
        public Laser(Battlefield field, Coor pos) : base(field, pos)
        {
            Symbols = "||";
            FgColor = ConsoleColor.White;
            BgColor = ConsoleColor.Black;
        }
    }

    class Shot: Projectile
    {
        public Shot(Battlefield field, Coor pos) : base(field, pos)
        {
            Symbols = "##";
            FgColor = ConsoleColor.White;
            BgColor = ConsoleColor.Black;
        }
    }

    class Beam: Projectile
    {
        public Beam(Battlefield field, Coor pos) : base(field, pos)
        {
            Symbols = "^^";
            FgColor = ConsoleColor.Black;
            BgColor = ConsoleColor.Cyan;
        }
    }

    class Load: Projectile
    {
        public Load(Battlefield field, Coor pos) : base(field, pos)
        {
            Symbols = "**";
            FgColor = ConsoleColor.Black;
            BgColor = ConsoleColor.Magenta;
        }
    }

    class Bullet: Projectile
    {
        public Bullet(Battlefield field, Coor pos) : base(field, pos)
        {
            Symbols = "**";
            FgColor = ConsoleColor.White;
            BgColor = ConsoleColor.Black;
        }
    }
}
