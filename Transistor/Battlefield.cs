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

        public Battlefield(string file)
        {
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
            enemyList = new EnemyList();

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
                    //jugador
                    break;
                case 'C':
                    Creep enemy = new Creep(this, new Coor(row, col));
                    enemyList.Append(enemy);
                    break;
                case 'S':
                    Snapshot enemy2 = new Snapshot(this, new Coor(row, col));
                    break;
                    // add enemies and player :TODO
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
                // execute one by one Turn :TODO
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

            //Ahora dibujamos los personajes
            for (int k = 0; k < CountEnemies(); k++) // Add CuentaEltos to EnemyList :TODO
            {
                PrintCharacters(c, k); // Recorrer la lista de enemigos y pasar el objeto :TODO
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
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("  ");
                    break;
                case Tile.Wall:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("**");
                    break;
                case Tile.BorderWall:
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("  ");
                    break;
            }
        }

        private void PrintCharacters(Character c, int num)
        {
            Console.SetCursorPosition(2 * c.pos.col, c.pos.fil);
            Console.BackgroundColor = colors[num]; // crear array de colores :TODO
            Console.ForegroundColor = ConsoleColor.White;

            if (num == 0) 
            {
                Console.ForegroundColor = ConsoleColor.Black;

                if (c.dir.col == 0 && c.dir.fil == 1)
                {
                    Console.Write("|@");
                }
                else if (c.dir.col == 0 && c.dir.fil == -1)
                {
                    Console.Write("@@");
                }
                else if (c.dir.col == 1 && c.dir.fil == 0)
                {
                    Console.Write("@|");
                }
                else if (c.dir.col == -1 && c.dir.fil == 0)
                {
                    Console.Write("||");
                }
                else 
                {
                    Console.Write("<>");
                }
            }
            else //if (c) Recorrer lista de enemigos para dibujar cada uno :TODO
            {
                Console.Write("ºº");
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }

        private void PrintAttacks()
        {
            // Recorrer lista de proyectiles para dibujar cada uno :TODO
        }

        private void PrintTurn()
        {
            // Dibujar la barra de Turn() :TODO
        }
    }
}
