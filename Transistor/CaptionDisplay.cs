using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{

    /*
     *  Ejemplo de uso
     *  
     
     CaptionDisplay captionDisplay = new CaptionDisplay(field.numRows, field.numCols);


     // Dentro del bucle de juego
     captionDisplay.Show();
          
     * 
     */



    class CaptionDisplay
    {
        private int topRow;
        private int maxColumns;
        private int level;
        Player player = new Player(null, null);
        Creep creep= new Creep(null, null);
        Snapshot snapshot= new Snapshot(null, null);
        Jerk jerk = new Jerk(null, null);
        Fetch fetch = new Fetch(null, null);
        Beam beam = new Beam(null, null, null, 0);
        Laser laser = new Laser(null, null, null, 0);
        Shot shot= new Shot(null, null, null, 0);
        Load load = new Load(null, null, null, 0);
        Bullet bullet = new Bullet(null, null, null, 0);


        public CaptionDisplay(int topRow, int maxColumns, int level)
        {
            this.topRow = topRow;
            this.maxColumns = maxColumns * 2;
            this.level = level;
            
        }

        public void Show()
        {
            int col;
            string header = new string(' ', 35);
            int row = 0;
            int initcol = maxColumns + 1;

            col = initcol;
            
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White);
            Console.Write(header);

            Console.SetCursorPosition(initcol + 13, row);
            SetColor(ConsoleColor.White, ConsoleColor.DarkRed);
            Console.Write("LEVEL " + level);

            col = initcol; row++;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White);
            Console.Write(header);

            

            

            // Fila 1
            row++;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White);
            Console.Write(header);
            col++;
            Console.SetCursorPosition(col, row);
            SetColor(player.BgColor, player.FgColor);
            Console.Write(player.Symbols);
            col += 4;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Write("Red (Player)");

            col += 15;
            Console.SetCursorPosition(col, row);
            SetColor(creep.BgColor, creep.FgColor);
            Console.Write(creep.Symbols);
            col += 4;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Write("Creep");

            //separador
            row++;
            col = initcol;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White);
            Console.Write(header);

            // Fila 2
            row++;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White);
            Console.Write(header);
            col++;
            Console.SetCursorPosition(col, row);
            SetColor(snapshot.BgColor, snapshot.FgColor);
            Console.Write(snapshot.Symbols);
            col += 4;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Write("Snapshot");

            col += 15;
            Console.SetCursorPosition(col, row);
            SetColor(jerk.BgColor, jerk.FgColor);
            Console.Write(jerk.Symbols);
            col += 4;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Write("Jerk");


            //separador
            row++;
            col = initcol;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White);
            Console.Write(header);

            // Fila 3
            row++;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White);
            Console.Write(header);
            col++;
            Console.SetCursorPosition(col, row);
            SetColor(fetch.BgColor, fetch.FgColor);
            Console.Write(fetch.Symbols);
            col += 4;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Write("Fetch");

            col += 15;
            Console.SetCursorPosition(col, row);
            SetColor(beam.BgColor, beam.FgColor);
            Console.Write(beam.Symbols);
            col += 4;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Write("Beam");

            //separador
            row++;
            col = initcol;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White);
            Console.Write(header);

            // Fila 4
            row++;
            col = initcol;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White);
            Console.Write(header);
            col++;
            Console.SetCursorPosition(col, row);
            SetColor(laser.BgColor, laser.FgColor);
            Console.Write(laser.Symbols);
            col += 4;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Write("Laser");

            col += 15;
            Console.SetCursorPosition(col, row);
            SetColor(shot.BgColor, shot.FgColor);
            Console.Write(shot.Symbols);
            col += 4;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Write("Shot");

            //separador
            row++;
            col = initcol;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White);
            Console.Write(header);
            // Fila 5
            row++;
            col = initcol;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White);
            Console.Write(header);
            col++;
            Console.SetCursorPosition(col, row);
            SetColor(load.BgColor, load.FgColor);
            Console.Write(load.Symbols);
            col += 4;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Write("Load");

            col += 15;
            Console.SetCursorPosition(col, row);
            SetColor(bullet.BgColor, bullet.FgColor);
            Console.Write(bullet.Symbols);
            col += 4;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Write("Bullet");

            //separador
            row++;
            col = initcol;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White);
            Console.Write(header);

        }

        void SetColor(ConsoleColor backColor = ConsoleColor.Black, ConsoleColor foreColor = ConsoleColor.White)
        {
            Console.BackgroundColor = backColor;
            Console.ForegroundColor = foreColor;
        }
    }
}
