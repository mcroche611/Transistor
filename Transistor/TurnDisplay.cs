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
        int BarLength = 20;
        Battlefield field;
        //bool crashEnabled = true;
        //bool breachEnabled = true;
        //bool loadEnabled = true;
        //bool pingEnabled = true;
        


        public TurnDisplay(Battlefield field, int topRow, int maxColumns)
        {
            this.field = field;
            this.topRow = topRow;
            this.maxColumns = maxColumns*2;
            BarLength =(int) (this.maxColumns * 0.7f);
        }

        //public bool CrashEnabled { get => crashEnabled; set => crashEnabled = value; }
        //public bool BreachEnabled { get => breachEnabled; set => breachEnabled = value; }
        //public bool LoadEnabled { get => loadEnabled; set => loadEnabled = value; }
        //public bool PingEnabled { get => pingEnabled; set => pingEnabled = value; }

        public void Show(TurnMode mode, float turnPercentage, float lifePercentage)
        {
            string header = new string(' ', maxColumns); //Separador

            int row = topRow;
            int col = 0;
            if (lifePercentage < 0)
                lifePercentage = 0;

            ConsoleColor bgColor = ConsoleColor.Black;
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

            row++; col = 0;
            ShowBar("Turn", turnPercentage, row, col, ConsoleColor.DarkBlue, bgColor);

            //separador
            row++; col = 0;
            Console.SetCursorPosition(col, row);
            SetColor();
            Console.Write(header);

            // Life Bar
            row++; col = 0;
            ShowBar("Life", lifePercentage, row, col, ConsoleColor.DarkRed);

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

            //Pinta casilla Crash
            label = "Crash: 1";
            ShowAttackLabel(row, col, label, 0, ConsoleColor.DarkYellow);
            col += label.Length + 2; //Pasa a la siguiente columna

            //Pinta casilla Breach
            label = "Breach: 2";
            ShowAttackLabel(row, col, label, 1, ConsoleColor.Cyan);
            col += label.Length + 2; //Pasa a la siguiente columna

            //Pinta casilla Ping
            label = "Ping: 3";
            ShowAttackLabel(row, col, label, 2, ConsoleColor.DarkGreen);
            col += label.Length + 2; //Pasa a la siguiente columna

            //Pinta casilla Load
            label = "Load: 4";
            ShowAttackLabel(row, col, label, 3, ConsoleColor.Magenta);
            col += label.Length + 2; //Pasa a la siguiente columna

            Console.SetCursorPosition(0, row + 10);
            SetColor(ConsoleColor.Black, ConsoleColor.Black);
        }

        private void ShowBar(string barName, float barPercentage, int row, int col, ConsoleColor barColor, ConsoleColor bgColor = ConsoleColor.Black)
        {
            string header = new string(' ', maxColumns);
            int resto;
            SetColor();
            Console.SetCursorPosition(col, row);
            Console.Write(header);

            col += 5;
            Console.SetCursorPosition(col, row);
            
            Console.Write(barName + ":");

            col += 5;
            Console.SetCursorPosition(col, row);
            int barChars = (int)(BarLength * barPercentage / 100);
            Console.BackgroundColor = barColor;
            Console.Write(new string(' ', barChars));
            Console.BackgroundColor = bgColor;

            resto = maxColumns - barChars - col;

            if (resto <= 0)
                resto = 1;

            Console.Write(new string(' ', resto));
        }

        private void ShowAttackLabel(int row, int col, string label, int attackNumber, ConsoleColor attackColor)
        {
            ConsoleColor boxBackground;

            if (field.Red.AttacksEnabled[attackNumber])
                boxBackground = attackColor;
            else
                boxBackground = ConsoleColor.DarkGray;

            SetColor(boxBackground, ConsoleColor.Black);
            Console.SetCursorPosition(col, row);
            Console.Write(label);
        }

        void SetColor(ConsoleColor backColor = ConsoleColor.Black, ConsoleColor foreColor = ConsoleColor.White)
        {
            Console.BackgroundColor = backColor;
            Console.ForegroundColor = foreColor;
        }
    }
}
