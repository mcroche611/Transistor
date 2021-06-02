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
        

        public Projectile(Battlefield field, Coor pos, Coor dir)
        {
            this.field = field;
            Pos = pos;
            Dir = dir;
        }

        public Coor Pos
        {
            get => pos;
            set => pos = value;
        }

        public Coor Dir
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

        public virtual bool Next(out Coor newPos)
        {
            newPos = Pos + Dir;

            bool possible = field.tile[newPos.row, newPos.col] == Battlefield.Tile.Empty;

            return possible;
        }

        public virtual void Move()
        {
            if (Next(out Coor newPos))
                Pos = newPos;
        }
    }

    class Laser: Projectile
    {
        public Laser(Battlefield field, Coor pos, Coor dir) : base(field, pos, dir)
        {
            Symbols = "||";
            FgColor = ConsoleColor.White;
            BgColor = ConsoleColor.Black;
        }
    }

    class Shot: Projectile
    {
        public Shot(Battlefield field, Coor pos, Coor dir) : base(field, pos, dir)
        {
            Symbols = "##";
            FgColor = ConsoleColor.White;
            BgColor = ConsoleColor.Black;
        }
    }

    class Beam: Projectile
    {
        public Beam(Battlefield field, Coor pos, Coor dir) : base(field, pos, dir)
        {
            Symbols = "^^";
            FgColor = ConsoleColor.Black;
            BgColor = ConsoleColor.Cyan;
        }
    }

    class Load: Projectile
    {
        public Load(Battlefield field, Coor pos, Coor dir) : base(field, pos, dir)
        {
            Symbols = "**";
            FgColor = ConsoleColor.Black;
            BgColor = ConsoleColor.Magenta;
        }

        public override void Move()
        {
            // No se mueve
        }
    }

    class Bullet: Projectile
    {
        public Bullet(Battlefield field, Coor pos, Coor dir) : base(field, pos, dir)
        {
            Symbols = "**";
            FgColor = ConsoleColor.White;
            BgColor = ConsoleColor.Black;
        }
    }
}
