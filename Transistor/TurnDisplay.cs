using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor


/*
  *  Ejemplo de uso
  *  

  TurnDisplay turnDisplay = new TurnDisplay(field.numRows, field.numCols);
  float turnPercentage = 100f;

  // Dentro del bucle de juego
  turnDisplay.Show(TurnMode.Normal, turnPercentage, 2,4,0,3);
  turnPercentage -= 0.5f;
  if (turnPercentage <= 0)
      turnPercentage = 100;


  * 
  */

{
    class TurnDisplay
    {
        private int topRow;
        private int maxColumns;
        int TurnLength = 20;
        bool crashEnabled = true;
        bool breachEnabled = true;
        bool loadEnabled = true;
        bool pingEnabled = true;
        public TurnDisplay(int topRow, int maxColumns)
        {
            this.topRow = topRow;
            this.maxColumns = maxColumns*2;
            TurnLength =(int) (this.maxColumns * 0.7f);
        }

        public bool CrashEnabled { get => crashEnabled; set => crashEnabled = value; }
        public bool BreachEnabled { get => breachEnabled; set => breachEnabled = value; }
        public bool LoadEnabled { get => loadEnabled; set => loadEnabled = value; }
        public bool PingEnabled { get => pingEnabled; set => pingEnabled = value; }

        public void Show(TurnMode mode, float turnPercentage, float lifePercentage, int crash, int breach, int load, int ping)
        {

            int row = topRow ;
            int col = 0;
            if (lifePercentage < 0)
                lifePercentage = 0;

            ConsoleColor bgColor= ConsoleColor.Black;
            switch (mode)
            {
                case TurnMode.Normal:
                    bgColor = ConsoleColor.Black;
                    break;
                case TurnMode.Plan:
                    bgColor = ConsoleColor.DarkGray;
                    break;
                case TurnMode.Run:
                    bgColor = ConsoleColor.DarkMagenta;
                    break;
            }
            
            //row = 1;
            //col = maxColumns + 1;

            ///// Pintar barra de Turn
            ///

            Console.SetCursorPosition(col, row);
            string header = new string(' ', maxColumns);
            Console.BackgroundColor = bgColor;
            Console.Write(header);
            // Turn Bar
            row++;
            Console.SetCursorPosition(col, row);
            Console.Write(header);
            col += 5;
            Console.SetCursorPosition(col, row);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Turn:");
            col += 5;
            Console.SetCursorPosition(col, row);
            int turnChars = (int )(TurnLength * turnPercentage/100);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(new string(' ',turnChars)); 
            Console.BackgroundColor = bgColor;
            int resto = maxColumns - turnChars - col;
            if (resto <= 0)
                resto = 1;
            Console.Write(new string(' ', resto));

            //separador
            row++; col = 0;
            Console.SetCursorPosition(col, row);
            SetColor();
            Console.Write(header);

            // Life Bar
            row++; col = 0;
            Console.SetCursorPosition(col, row);
            Console.Write(header);
            col += 5;
            Console.SetCursorPosition(col, row);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Life:");
            col += 5;
            Console.SetCursorPosition(col, row);
            int lifeChars = (int)(TurnLength * lifePercentage / 100);
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write(new string(' ', lifeChars));
            Console.BackgroundColor = bgColor;
            resto = maxColumns - turnChars - col;
            if (resto <= 0)
                resto = 1;
            Console.Write(new string(' ', resto));

            //separador
            row++; col = 0;
            Console.SetCursorPosition(col, row);
            SetColor();
            Console.Write(header);

            // Pintar ataques
            ///////////////////////////////////
            row++;
            col = 5;

            string label;

            ConsoleColor boxBackground;
            if (crashEnabled)
                boxBackground = ConsoleColor.DarkYellow;
            else
                boxBackground = ConsoleColor.DarkGray;
            SetColor(boxBackground, ConsoleColor.Black);
            Console.SetCursorPosition(col, row);
            label = "Crash: " + crash;
            Console.Write(label);
            col += label.Length + 2;

            if (breachEnabled)
                boxBackground = ConsoleColor.Cyan;
            else
                boxBackground = ConsoleColor.DarkGray;
            SetColor(boxBackground, ConsoleColor.Black);            
            Console.SetCursorPosition(col, row);
            label = "Breach: " + breach;
            Console.Write(label);
            col += label.Length + 2;

            if (pingEnabled)
                boxBackground = ConsoleColor.DarkGreen;
            else
                boxBackground = ConsoleColor.DarkGray;
            SetColor(boxBackground, ConsoleColor.Black);            
            Console.SetCursorPosition(col, row);
            label = "Ping: " + ping;
            Console.Write(label);
            col += label.Length + 2;

            if (loadEnabled)
                boxBackground = ConsoleColor.Magenta;
            else
                boxBackground = ConsoleColor.DarkGray;
            SetColor(boxBackground, ConsoleColor.Black);            
            Console.SetCursorPosition(col, row);
            label = "Load: " + load;
            Console.Write(label);
            col += label.Length + 2;

            Console.SetCursorPosition(0, row + 10);
            SetColor(ConsoleColor.Black, ConsoleColor.Black);
        }
        void SetColor(ConsoleColor backColor = ConsoleColor.Black, ConsoleColor foreColor = ConsoleColor.White)
        {
            Console.BackgroundColor = backColor;
            Console.ForegroundColor = foreColor;
        }
    }
}
