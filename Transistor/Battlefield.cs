using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Transistor
{
    class Battlefield
    {
        public enum Tile { Empty, Wall, BorderWall };
        public Tile[,] tile; //matriz de casillas del nivel.
        private int numCols;
        private int numRows;

        public SoundFX Fx = new SoundFX();
        private float turnPercentage = 100f;

        EnemyList enemyList; 
        Player red;
        ProjectileList projectileList;

        internal EnemyList EnemyList { get => enemyList;}
        internal Player Red { get => red;}
        internal ProjectileList ProjectileList { get => projectileList;}
        public float TurnPercentage { get => turnPercentage; set => turnPercentage = value; }
        public int NumRows { get => numRows; set => numRows = value; }
        public int NumCols { get => numCols; set => numCols = value; }

        public Battlefield(string file)
        {
            enemyList = new EnemyList();
            projectileList = new ProjectileList();

            StreamReader streamReader = new StreamReader(file);

            bool levelComplete = false;
            string s = "";
            numRows = 0;
            numCols = 0;

            //Lee el nivel del archivo
            while (!streamReader.EndOfStream && !levelComplete)
            {
                string line = streamReader.ReadLine();

                if (line == "")
                {
                    levelComplete = true;
                }
                else
                {
                    int rows = 0;

                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] != ' ')
                        {
                            s += line[i];
                            rows++;
                        }
                    }
                    numRows++;
                    if (numCols < rows)
                        numCols = rows;
                }
            }
            streamReader.Close();

            tile = new Tile[numRows, numCols];

            FillField(s, ref tile);
        }

        //Rellena la matriz de casillas
        private void FillField(string s, ref Tile[,] tile)
        {
            int col = 0, row = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (col >= numCols)
                {
                    row++;
                    col = 0;
                }

                CharToTileAndCharacter(s[i], ref tile[row, col], row, col);
                col++;
            }
        }

        //Asigna cada casilla y personaje
        private void CharToTileAndCharacter(char symbol, ref Tile tile, int row, int col)
        {
            switch (symbol)
            {
                case '0':
                    tile = Tile.Empty;
                    break;
                case '1':
                    tile = Tile.Wall;
                    break;
                case '2':
                    tile = Tile.BorderWall;
                    break;
                case 'R':
                    red = new Player(this, new Coor(row, col));
                    break;
                case 'C':
                    {
                        Creep enemy = new Creep(this, new Coor(row, col));
                        EnemyList.Append(enemy);
                        break;
                    }
                case 'S':
                    {
                        Snapshot enemy = new Snapshot(this, new Coor(row, col));
                        EnemyList.Append(enemy);
                        break;
                    }
                case 'J':
                    {
                        Jerk enemy = new Jerk(this, new Coor(row, col));
                        EnemyList.Append(enemy);
                        break;
                    }
                case 'F':
                    {
                        Fetch enemy = new Fetch(this, new Coor(row, col));
                        EnemyList.Append(enemy);
                        break;
                    }
            }
        }

        //Pinta el tablero, personajes y proyectiles en pantalla
        public void Show(TurnMode mode, char currentAttack) 
        {
            //Primero dibuja el tablero vacio
            for (int i = 0; i < tile.GetLength(0); i++)
            {
                for (int j = 0; j < tile.GetLength(1); j++)
                {
                    PrintTile(tile[i, j], i, j);
                }
            }

            // Dibuja los proyectiles
            for (int k = 0; k < ProjectileList.Count(); k++)
            {
                Projectile projectile = ProjectileList.nEsimo(k);
                PrintProjectiles(projectile);
            }

            // Dibuja al jugador
            PrintCharacter(Red, mode, currentAttack);

            // Dibuja los enemigos
            for (int k = 0; k < EnemyList.Count(); k++)
            {
                Enemy enemy = EnemyList.nEsimo(k);
                PrintCharacter(enemy);
            }

            Console.SetCursorPosition(0, numRows + 2);
        }

        public void MoveEnemies(int lapCounter, TurnMode mode = TurnMode.Normal)
        {
            if (mode == TurnMode.Normal) //Solo se mueven en el modo normal
            {
                for (int k = 0; k < EnemyList.Count(); k++)
                {
                    Enemy enemy = EnemyList.nEsimo(k);
                    if (lapCounter % enemy.Speed == 0)
                        enemy.Move(mode);
                }
            }

            enemyList.DeleteDestroyed();
        }

        public void EnemiesAttack(TurnMode mode = TurnMode.Normal) //parámetro para pasar al Attack() de Character que necesita mode para Player
        {
            for (int k = 0; k < EnemyList.Count(); k++)
            {
                Enemy enemy = EnemyList.nEsimo(k);
                enemy.Attack(mode, ' ');
            }
        }

        public void MoveProjectiles(TurnMode mode = TurnMode.Normal)
        {
            for (int k = 0; k < ProjectileList.Count(); k++)
            {
                Projectile projectile = ProjectileList.nEsimo(k);

                bool needMove = mode == TurnMode.Normal || (mode == TurnMode.Run && projectile.PlayerOwned);

                if (needMove)
                {
                    projectile.Move();
                    projectile.CheckDamage();
                }
            }

            projectileList.DeleteDestroyed();
        }

        private void PrintTile(Tile tile, int row, int col)
        {
            if (!ElementInPos(new Coor(row, col)))
            {
                Console.SetCursorPosition(2 * col, row);
                Console.BackgroundColor = ConsoleColor.Black;

                switch (tile)
                {
                    case Tile.Empty:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write("  ");
                        break;
                    case Tile.Wall:
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("  ");
                        break;
                    case Tile.BorderWall:
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("  ");
                        break;
                }
            }
        }

        private void PrintCharacter(Character c, TurnMode mode = TurnMode.Normal, char currentAttack = ' ')
        {
            if (c.PosChanged)
            {
                Console.SetCursorPosition(2 * c.Pos.col, c.Pos.row);
                SetColor(c.BgColor, c.FgColor);
                Console.Write(c.Symbols);

                c.Painted();
                Console.BackgroundColor = ConsoleColor.Black;
            }

            if (c is Jerk)
            {
                PrintRange(c);
            }

            if (mode == TurnMode.Plan && c is Player && currentAttack != ' ')
            {
                PrintAim(currentAttack);
            }
        }

        private void PrintProjectiles(Projectile p)
        {
            if (!enemyList.IsEnemy(p.Pos))
            {
                Console.SetCursorPosition(2 * p.Pos.col, p.Pos.row);
                SetColor(p.BgColor, p.FgColor);
                Console.Write(p.Symbols);

                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        //Dibuja el área de ataque del enemigo Jerk
        private void PrintRange(Character c)
        {
            int range = 2;

            for (int j = -range; j <= range; j++)
            {
                int col;
                if (j <= 0)
                    col = (range + j);
                else
                    col = (range - j);

                for (int k = -col; k <= col; k++)
                {
                    Coor newPos = c.Pos + new Coor(j, k);

                    if (newPos.row < tile.GetLength(0) && newPos.col < tile.GetLength(1) && newPos.row > 0 && newPos.col > 0)
                    {
                        if (newPos != Red.Pos && !enemyList.IsEnemy(newPos) && tile[newPos.row, newPos.col] == Tile.Empty && newPos != c.Pos)
                        {
                            Console.SetCursorPosition(2 * newPos.col, newPos.row);
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write("  ");
                        }
                    }
                }
            }
        }

        //Dibuja el ataque posible durante la planificación de Turn
        public void PrintAim(char attack)
        {
            Coor newPos;

            switch (attack)
            {
                case 'c':
                    if (red.Next(out newPos))
                    {
                        Console.SetCursorPosition(2 * newPos.col, newPos.row);
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("  ");
                    }
                    break;
                case 'b':
                    {
                        newPos = red.Pos;

                        while (NextDir(red.Dir, newPos, attack, out newPos))
                        {
                            if (!enemyList.IsEnemy(newPos) && !projectileList.IsProjectile(newPos))
                            {
                                Console.SetCursorPosition(2 * newPos.col, newPos.row);
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Console.Write("  ");
                            }
                        }
                    }
                    break;
                case 'p':
                    {
                        newPos = red.Pos;

                        // Puede seguir si no se choca con ninguna pared o enemigo
                        while (NextDir(red.Dir, newPos, attack, out newPos))
                        {
                            if (!enemyList.IsEnemy(newPos) && !projectileList.IsProjectile(newPos))
                            {
                                Console.SetCursorPosition(2 * newPos.col, newPos.row);
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Console.Write("  ");
                            }
                        }
                    }
                    break;
                case 'l':
                    {
                        if (red.Next(out newPos))
                        {
                            Console.SetCursorPosition(2 * newPos.col, newPos.row);
                            SetColor(ConsoleColor.DarkMagenta, ConsoleColor.White); 
                            Console.Write("  ");
                        }
                    }
                    break;
            }
        }

        //Devuelve si está libre la siguiente posición para un ataque
        public bool NextDir(Coor dir, Coor pos, char attack, out Coor newPos)
        {
            newPos = pos + dir;

            bool possible;

            if (attack == 'b')
            {
                possible = tile[newPos.row, newPos.col] != Battlefield.Tile.BorderWall;
            }
            else 
            {
                possible = tile[newPos.row, newPos.col] == Battlefield.Tile.Empty && !enemyList.IsEnemy(newPos);
            }

            return possible;
        }

        //Devuelve si hay un objeto en la posición
        private bool ElementInPos(Coor pos)
        {
            bool elementFound = false;
            elementFound = red.Pos == pos;
            if (!elementFound)
            {
                elementFound = enemyList.IsEnemy(pos);
                if (!elementFound)
                    elementFound = projectileList.IsProjectile(pos);
            }

            return elementFound;
        }

        internal void DestroyWall(Coor pos)
        {
            tile[pos.row, pos.col] = Tile.Empty;
        }

        protected void SetColor(ConsoleColor bgColor = ConsoleColor.Black, ConsoleColor fgColor = ConsoleColor.White)
        {
            Console.BackgroundColor = bgColor;
            Console.ForegroundColor = fgColor;
        }

        public bool InLimits(Coor pos)
        {
            return (pos.row < tile.GetLength(0) && pos.col < tile.GetLength(1) && pos.row > 0 && pos.col > 0);
        }
    }
}
