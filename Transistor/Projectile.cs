using System;

namespace Transistor
{
    class Projectile
    {
        private Coor dir;
        private Coor pos;
        protected int damage;
        protected bool destroyed;
        protected bool playerOwned;
        private string symbols;
        private ConsoleColor fgColor;
        private ConsoleColor bgColor;
        protected Battlefield field;
        

        public Projectile(Battlefield field, Coor pos, Coor dir, int damage)
        {
            this.field = field;
            Pos = pos;
            Dir = dir;
            playerOwned = false;
            this.damage = damage; 
            destroyed = false;
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

        public bool Destroyed 
        { 
            get => destroyed; 
        }

        public bool PlayerOwned
        {
            get => playerOwned;
        }

        public virtual bool Next(out Coor newPos)
        {
            
            newPos = Pos + Dir;            
            bool possible = field.tile[newPos.row, newPos.col] == Battlefield.Tile.Empty;

            return possible;
        }

        public virtual void Move()
        {
            Next(out Coor newPos);
            Pos = newPos;
        }

        public virtual void CheckDamage()
        {            
            if (Pos == field.Red.Pos)
            {
                field.Red.ReceiveDamage(damage);
                destroyed = true;
            }
            else if (field.tile[Pos.row, Pos.col] != Battlefield.Tile.Empty)
            {
                destroyed = true;
            }
            else if (field.ProjectileList.IsProjectile(Pos) && field.ProjectileList.GetProjectileInPos(Pos) is Load) //TOCHECK: Como todos lo tienen pero solo lo usa Load, hace falta validarlo?
            {
                Projectile p = field.ProjectileList.GetProjectileInPos(Pos);

                if (p is Load)
                {
                    p.ReceiveDamage();
                    destroyed = true;
                }
            }
        }

        public virtual void ReceiveDamage()
        {
            // Solo va a usarlo Load
        }
    }

    class Laser: Projectile
    {
        public Laser(Battlefield field, Coor pos, Coor dir, int damage) : base(field, pos, dir, damage)
        {
            Symbols = "||";
            FgColor = ConsoleColor.White;
            BgColor = ConsoleColor.Black;
        }
    }

    class Shot: Projectile
    {
        public Shot(Battlefield field, Coor pos, Coor dir, int damage) : base(field, pos, dir, damage)
        {
            Symbols = "##";
            FgColor = ConsoleColor.White;
            BgColor = ConsoleColor.Black;
        }
    }

    class Beam: Projectile
    {
        public Beam(Battlefield field, Coor pos, Coor dir, int damage) : base(field, pos, dir, damage)
        {
            Symbols = "[]";
            FgColor = ConsoleColor.Black;
            BgColor = ConsoleColor.Cyan;
            playerOwned = true;
        }

        public override void CheckDamage()
        {
            if (field.EnemyList.IsEnemy(Pos))
            {
                Enemy e = field.EnemyList.GetEnemyInPos(Pos);
                e.ReceiveDamage(damage);
            }   
            else if (field.tile[Pos.row, Pos.col] == Battlefield.Tile.BorderWall)
            {
                destroyed = true;
            }
            else if (field.tile[Pos.row, Pos.col] == Battlefield.Tile.Wall)
            {
                field.DestroyWall(Pos);
            }
            else if (field.ProjectileList.IsProjectile(Pos) && field.ProjectileList.GetProjectileInPos(Pos) is Load) //TOCHECK: Como todos lo tienen pero solo lo usa Load, hace falta validarlo?
            {
                Projectile p = field.ProjectileList.GetProjectileInPos(Pos);

                if (p is Load)
                {
                    p.ReceiveDamage();
                    destroyed = true;
                }
            }
        }
    }

    class Load: Projectile
    {
        bool exploded = false;

        public Load(Battlefield field, Coor pos, Coor dir, int damage) : base(field, pos, dir, damage)
        {
            Symbols = "**";
            FgColor = ConsoleColor.Black;
            BgColor = ConsoleColor.Magenta;
            playerOwned = true;
        }

        public override void Move()
        {
            // No se mueve porque es una bomba
        }

        public override void CheckDamage()
        {
            if (exploded)
            {
                field.Fx.PlayExplosion();

                Coor newPos;

                int maxRange = 3;

                //Chequeo de enemigos en un área en rombo 
                for (int j = -maxRange; j <= maxRange; j++)
                {
                    int col;
                    if (j <= 0)
                        col = (maxRange + j);
                    else
                        col = (maxRange - j);

                    for (int k = -col; k <= col; k++)
                    {
                        newPos = Pos + new Coor(j, k);

                        if (field.InLimits(newPos))
                        {
                            //Pinta el área de la explosión momentáneamente
                            if (newPos != field.Red.Pos && !field.EnemyList.IsEnemy(newPos) && field.tile[newPos.row, newPos.col] == Battlefield.Tile.Empty)
                            {
                                Console.SetCursorPosition(2 * newPos.col, newPos.row);
                                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                                Console.Write("  ");
                            }

                            if (field.EnemyList.IsEnemy(newPos)) //No hace daño ni al jugador ni a otros load
                            {
                                Enemy e = field.EnemyList.GetEnemyInPos(newPos);
                                e.ReceiveDamage(damage);
                            }
                            else if (field.tile[newPos.row, newPos.col] == Battlefield.Tile.Wall)
                            {
                                field.DestroyWall(newPos);
                            }
                        }
                    }
                }

                destroyed = true;
            }
            //TODO: Hace falta validar/impedir que el jugador no tire el load() sobre un muro???
        }

        public override void ReceiveDamage()
        {
            exploded = true;
            CheckDamage();
        }
    }

    class Bullet: Projectile
    {
        public Bullet(Battlefield field, Coor pos, Coor dir, int damage) : base(field, pos, dir, damage)
        {
            Symbols = "**";
            FgColor = ConsoleColor.White;
            BgColor = ConsoleColor.Black;
            playerOwned = true;
        }

        public override void CheckDamage()
        {
            if (field.EnemyList.IsEnemy(Pos)) // Hace daño y se destruye al chocar con enemigos
            {
                Enemy e = field.EnemyList.GetEnemyInPos(Pos);
                e.ReceiveDamage(damage);
                destroyed = true;
            }
            else if (field.tile[Pos.row, Pos.col] != Battlefield.Tile.Empty) //Se destruye con paredes
            {
                destroyed = true;
            }
            else if (field.ProjectileList.IsProjectile(Pos) && field.ProjectileList.GetProjectileInPos(Pos) is Load) //TOCHECK: Como todos lo tienen pero solo lo usa Load, hace falta validarlo?
            {
                Projectile p = field.ProjectileList.GetProjectileInPos(Pos);

                if (p is Load)
                {
                    p.ReceiveDamage();
                    destroyed = true;
                }
            }
        }
    }
}
