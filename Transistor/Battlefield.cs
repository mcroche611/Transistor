using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Transistor
{
    class Battlefield
    {
        enum Tile { Empty, Wall, BorderWall };
        Tile[,] tile; //matriz de casillas del nivel.
        int numRows, numCols;

        EnemyList enemyList;
        Player red;
        ProjectileList projectileList;


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
                        enemyList.Append(enemy);
                        break;
                    }
                case 'S':
                    {
                        Snapshot enemy = new Snapshot(this, new Coor(row, col));
                        enemyList.Append(enemy);
                        break;
                    }
                case 'J':
                    {
                        Jerk enemy = new Jerk(this, new Coor(row, col));
                        enemyList.Append(enemy);
                        break;
                    }
                case 'F':
                    {
                        Fetch enemy = new Fetch(this, new Coor(row, col));
                        enemyList.Append(enemy);
                        break;
                    }
            }
        }

        public void Show(TurnMode mode) //Llama a uno u otro renderizado de pantalla.
        {
            if (mode == TurnMode.normal)
            {
                ShowBattle();
            }
            else if (mode == TurnMode.plan)
            {
                ShowTurn();
            }
            else
            {
                // TODO: execute one by one Turn 
            }
        }

        private void ShowBattle()
        {
            //Primero dibuja el tablero vacio
            for (int i = 0; i < tile.GetLength(0); i++)
            {
                for (int j = 0; j < tile.GetLength(1); j++)
                {
                    PrintTile(tile[i, j], i, j);
                }
                Console.WriteLine();
            }

            // Dibuja al jugador
            PrintCharacter(red);

            // Dibuja los enemigos
            for (int k = 0; k < enemyList.Count(); k++) // TODO: Add CuentaEltos to EnemyList 
            {
                Enemy enemy = enemyList.nEsimo(k);
                PrintCharacter(enemy); // TODO: Recorrer la lista de enemigos y pasar el objeto 
            }

            // Dibuja los proyectiles
            for (int k = 0; k < projectileList.Count(); k++) // TODO: Add CuentaEltos to EnemyList 
            {
                Projectile projectile = projectileList.nEsimo(k);
                PrintAttacks(projectile); // TODO: Recorrer la lista de enemigos y pasar el objeto 
            }

            Console.SetCursorPosition(0, numRows + 2);

            //if (Debug)
            //{
            //    Console.BackgroundColor = ConsoleColor.Black;
            //    Console.ForegroundColor = ConsoleColor.White;

            //    Console.WriteLine("Comida Restante: {0}", numComida);
            //    for (int i = 0; i < pers.Length; i++)
            //    {
            //        Console.ForegroundColor = colors[i];
            //        Console.WriteLine("{0} --> Pos: ({1}, {2})      Dir: ({3}, {4}) ", pers[i].name, pers[i].pos.fil, pers[i].pos.col, pers[i].dir.fil, pers[i].dir.col);
            //    }
            //}
        }

        private void ShowTurn()
        {

        }

        private void PrintTile(Tile tile, int row, int col)
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

        private void PrintCharacter(Character c)
        {
            Console.SetCursorPosition(2 * c.Pos.col, c.Pos.fil);
            Console.ForegroundColor = ConsoleColor.White; //TODO: Add property foreground color

            Console.BackgroundColor = c.Color;
            Console.Write(c.Symbols);

            Console.BackgroundColor = ConsoleColor.Black;
        }

        private void PrintAttacks(Projectile p)
        {
            Console.SetCursorPosition(2 * p.Pos.col, p.Pos.fil);
            Console.ForegroundColor = p.FgColor;

            Console.BackgroundColor = p.BgColor;
            Console.Write(p.Symbols);

            Console.BackgroundColor = ConsoleColor.Black;
        }

        private void PrintTurn()
        {
            // TODO: Dibujar la barra de Turn()
        }
    }
}
