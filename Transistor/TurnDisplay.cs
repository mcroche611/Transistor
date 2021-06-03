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

        public TurnDisplay(int topRow, int maxColumns)
        {
            this.topRow = topRow;
            this.maxColumns = maxColumns*2;
            TurnLength =(int) (this.maxColumns * 0.7f);
        }
        public void Show(TurnMode mode, float turnPercentage, int crash, int breach, int load, int ping)
        {

            int row = topRow + 1;
            int col = 0;

            //row = 1;
            //col = maxColumns + 1;

            ///// Pintar barra de Turn
            ///

            Console.SetCursorPosition(col, row);
            string header = new string(' ', maxColumns);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(header);
            row++;
            col += 5;
            Console.SetCursorPosition(col, row);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Turn:");
            col += 5;
            Console.SetCursorPosition(col, row);
            int turnChars = (int )(TurnLength * turnPercentage/100);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(new string(' ',turnChars)); 
            Console.BackgroundColor = ConsoleColor.Black;
            int resto = maxColumns - turnChars - col;
            if (resto <= 0)
                resto = 1;
            Console.Write(new string(' ', resto));
            row++;
            Console.SetCursorPosition(col, row);
            SetColor();
            Console.Write(header);
            
            // Pintar ataques
            ///////////////////////////////////
            row++;
            col = 5;

            string label;
            
            SetColor(ConsoleColor.DarkYellow, ConsoleColor.Black);
            Console.SetCursorPosition(col, row);
            label = "Crash: " + crash;
            Console.Write(label);
            col += label.Length + 2;

            SetColor(ConsoleColor.Cyan, ConsoleColor.Black);
            Console.SetCursorPosition(col, row);
            label = "Breach: " + breach;
            Console.Write(label);
            col += label.Length + 2;

            SetColor(ConsoleColor.DarkGreen, ConsoleColor.Black);
            Console.SetCursorPosition(col, row);
            label = "Ping: " + ping;
            Console.Write(label);
            col += label.Length + 2;

            SetColor(ConsoleColor.Magenta, ConsoleColor.Black);
            Console.SetCursorPosition(col, row);
            label = "Load: " + load;
            Console.Write(label);
            col += label.Length + 2;

        }
        void SetColor(ConsoleColor backColor= ConsoleColor.Black, ConsoleColor foreColor = ConsoleColor.White)
        {
            Console.BackgroundColor = backColor;
            Console.ForegroundColor = foreColor;
        }
    }
}
