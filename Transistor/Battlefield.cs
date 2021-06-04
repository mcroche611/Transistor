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
        public int numRows, numCols; //OJO TODO: Hacer propiedades
        public SoundFX Fx = new SoundFX();
        private float turnPercentage = 100f;

        EnemyList enemyList; 
        Player red;
        ProjectileList projectileList;

        internal EnemyList EnemyList { get => enemyList;}
        internal Player Red { get => red;}
        internal ProjectileList ProjectileList { get => projectileList;}
        public float TurnPercentage { get => turnPercentage; set => turnPercentage = value; }

        public Battlefield(string file)
        {
            enemyList = new EnemyList();
            projectileList = new ProjectileList();

            StreamReader streamReader = new StreamReader(file);

            bool levelComplete = false;
            string s = "";
            numRows = 0;
            numCols = 0;

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

        public void Show(TurnMode mode, char currentAttack) //Llama a uno u otro renderizado de pantalla.
        {
            //Primero dibuja el tablero vacio
            for (int i = 0; i < tile.GetLength(0); i++)
            {
                for (int j = 0; j < tile.GetLength(1); j++)
                {
                    PrintTile(tile[i, j], i, j);
                }
                //Console.WriteLine();
            }

            // Dibuja los proyectiles
            for (int k = 0; k < ProjectileList.Count(); k++)
            {
                Projectile projectile = ProjectileList.nEsimo(k);
                PrintAttacks(projectile);
            }

            // Dibuja al jugador
            PrintCharacter(Red, mode, currentAttack);

            //if (mode == TurnMode.Plan && currentAttack != ' ')
            //{
            //    PrintAim(Red, currentAttack);
            //}

            // Dibuja los enemigos
            for (int k = 0; k < EnemyList.Count(); k++)
            {
                Enemy enemy = EnemyList.nEsimo(k);
                PrintCharacter(enemy);
            }

            Console.SetCursorPosition(0, numRows + 2);


            //if (mode == TurnMode.Normal)
            //{
            //    ShowBattle();
            //}
            //else if (mode == TurnMode.Plan)
            //{
            //    ShowTurn();
            //}
            //else
            //{
            //    // TODO: execute one by one Turn 
            //}
        }

        //private void ShowBattle()
        //{
        //    //Primero dibuja el tablero vacio
        //    for (int i = 0; i < tile.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < tile.GetLength(1); j++)
        //        {
        //            PrintTile(tile[i, j], i, j);
        //        }
        //        //Console.WriteLine();
        //    }

        //    // Dibuja los proyectiles
        //    for (int k = 0; k < ProjectileList.Count(); k++)
        //    {
        //        Projectile projectile = ProjectileList.nEsimo(k);
        //        PrintAttacks(projectile);
        //    }

        //    // Dibuja al jugador
        //    PrintCharacter(Red);

        //    // Dibuja los enemigos
        //    for (int k = 0; k < EnemyList.Count(); k++)
        //    {
        //        Enemy enemy = EnemyList.nEsimo(k);
        //        PrintCharacter(enemy);
        //    }

        //    Console.SetCursorPosition(0, numRows + 2);

        //    //if (Debug)
        //    //{
        //    //    Console.BackgroundColor = ConsoleColor.Black;
        //    //    Console.ForegroundColor = ConsoleColor.White;

        //    //    Console.WriteLine("Comida Restante: {0}", numComida);
        //    //    for (int i = 0; i < pers.Length; i++)
        //    //    {
        //    //        Console.ForegroundColor = colors[i];
        //    //        Console.WriteLine("{0} --> Pos: ({1}, {2})      Dir: ({3}, {4}) ", pers[i].name, pers[i].pos.fil, pers[i].pos.col, pers[i].dir.fil, pers[i].dir.col);
        //    //    }
        //    //}
        //}

        public void MoveEnemies(TurnMode mode = TurnMode.Normal)
        {
            if (mode == TurnMode.Normal) //Solo se mueven en el modo normal
            {
                for (int k = 0; k < EnemyList.Count(); k++)
                {
                    Enemy enemy = EnemyList.nEsimo(k);
                    enemy.Move(mode);
                }
            }

            // Chequea si algún enemigo ha sido destruido y si es así, lo elimina
            for (int k = 0; k < EnemyList.Count(); k++)
            {
                Enemy enemy = EnemyList.nEsimo(k);
                
                if (enemy.Destroyed)
                {
                    enemyList.Delete(enemy);
                }
            }
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

            for (int k = 0; k < ProjectileList.Count(); k++)
            {
                Projectile projectile = ProjectileList.nEsimo(k);
                
                if (projectile.Destroyed)
                {
                    ProjectileList.BorraElto(projectile);
                }
            }
        }

        //private void ShowTurn()
        //{

        //}

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
                Console.SetCursorPosition(2 * c.Pos.col, c.Pos.row); //TODO: método que englobe estas tres líneas
                SetColor(c.BgColor, c.FgColor);
                Console.Write(c.Symbols);

                c.Painted();
                Console.BackgroundColor = ConsoleColor.Black;
            }

            //TODO: Design Choice, si se sitúan dentro se ven una vez por mov, si no, parpadean de forma constante
            if (c is Jerk)
            {
                PrintRange(c);
            }

            if (mode == TurnMode.Plan && c is Player && currentAttack != ' ')
            {
                PrintAim(c, currentAttack);
            }
        }

        private void PrintAttacks(Projectile p)
        {
            Console.SetCursorPosition(2 * p.Pos.col, p.Pos.row);
            Console.ForegroundColor = p.FgColor;

            Console.BackgroundColor = p.BgColor;
            Console.Write(p.Symbols);

            Console.BackgroundColor = ConsoleColor.Black;
        }

        private void PrintRange(Character c)
        {
            int range = 3;

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
                        if (newPos != Red.Pos && tile[newPos.row, newPos.col] == Tile.Empty && newPos != c.Pos)
                        {
                            Console.SetCursorPosition(2 * newPos.col, newPos.row);
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write("  ");
                        }
                    }
                }
            }
        }

        public void PrintAim(Character c, char attack)
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
                            SetColor(ConsoleColor.DarkMagenta, ConsoleColor.White); //TODO: set to color of projectile
                            Console.Write("  ");
                        }
                    }
                    break;
            }
        }

        //private int Range(Coor dir, Coor pos, char attack, out Coor rangePos)
        //{
        //    int newRange = 0;
        //    bool outOfBoard = false;
        //    rangePos = pos;

        //    while (!outOfBoard)
        //    {
        //        if (NextDir(dir, pos + new Coor(dir.row * newRange, dir.col * newRange), attack, out rangePos))
        //        {
        //            newRange++;
        //        }
        //        else
        //        {
        //            outOfBoard = true;
        //        }
        //    }

        //    return newRange;
        //}

        private bool NextDir(Coor dir, Coor pos, char attack, out Coor newPos)
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

        public Player GetPlayer()
        {
            return Red;
        }

        internal void DestroyWall(Coor pos)
        {
            tile[pos.row, pos.col] = Tile.Empty;
        }

        protected void SetColor(ConsoleColor bgColor = ConsoleColor.Black, ConsoleColor fgColor = ConsoleColor.White)
        {
            Console.BackgroundColor = bgColor; //TODO: A la hora de pintar
            Console.ForegroundColor = fgColor;
        }
    }
}
