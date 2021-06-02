using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{

    /*
     *  Ejemplo de uso
     *  
     
     CaptionDisplay captionDisplay= new CaptionDisplay(field.numRows, field.numCols);


     // Dentro del bucle de juego
     captionDisplay.Show();
          
     * 
     */



    class CaptionDisplay
    {
        private int topRow;
        private int maxColumns;
        Player player = new Player(null, null);
        Creep creep= new Creep(null, null);
        Snapshot snapshot= new Snapshot(null, null);
        Jerk jerk = new Jerk(null, null);
        Fetch fetch = new Fetch(null, null);
        Beam beam = new Beam(null, null, null);
        Laser laser = new Laser(null, null, null);
        Shot shot= new Shot(null, null, null);
        Load load = new Load(null, null, null);
        Bullet bullet = new Bullet(null, null, null);


        public CaptionDisplay(int topRow, int maxColumns)
        {
            this.topRow = topRow;
            this.maxColumns = maxColumns * 2;
            
        }

        public void Show()
        {
            string header = new string(' ', 35);
            int row = 1;
            int initcol = maxColumns + 1;

            int col = initcol;
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
            SetColor(player.Color);
            Console.Write(player.Symbols);
            col += 4;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Write("Red (Player)");

            col += 15;
            Console.SetCursorPosition(col, row);
            SetColor(creep.Color);
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
            SetColor(snapshot.Color);
            Console.Write(snapshot.Symbols);
            col += 4;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Write("Snapshot");

            col += 15;
            Console.SetCursorPosition(col, row);
            SetColor(jerk.Color);
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
            SetColor(fetch.Color);
            Console.Write(fetch.Symbols);
            col += 4;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Write("Fetch");

            col += 15;
            Console.SetCursorPosition(col, row);
            //SetColor(beam.Color);
            SetColor(ConsoleColor.Cyan);
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
            //SetColor(laser.Color);
            SetColor(ConsoleColor.Green);
            Console.Write(laser.Symbols);
            col += 4;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Write("Laser");

            col += 15;
            Console.SetCursorPosition(col, row);
            //SetColor(beam.Color);
            SetColor();
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
            //SetColor(laser.Color);
            SetColor(ConsoleColor.DarkMagenta);
            Console.Write(load.Symbols);
            col += 4;
            Console.SetCursorPosition(col, row);
            SetColor(ConsoleColor.White, ConsoleColor.Black);
            Console.Write("Load");

            col += 15;
            Console.SetCursorPosition(col, row);
            //SetColor(beam.Color);
            SetColor();
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
