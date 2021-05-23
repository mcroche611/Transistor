using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    enum TurnMode { normal, plan, run }

    class Transistor
    {
        TurnMode mode;

        EnemyList enemies;
        Battlefield field;
        Player red;

        void Run()
        {
            field = new Battlefield("nivel1");
            red = new Player(field, new Coor());
            enemies = new EnemyList();

        }

        void ReadInput(TurnMode mode)
        {
            if (mode == TurnMode.normal)
            {
                ReadInputBattle();
            }
        }

        private void ReadInputBattle()
        {
            if (Console.KeyAvailable)
            {
                string tecla = Console.ReadKey().Key.ToString();
                switch (tecla)
                {
                    case "LeftArrow":
                        red.Dir = new Coor(-1, 0);
                        break;
                    case "RightArrow":
                        red.Dir = new Coor(1, 0);
                        break;
                    case "UpArrow":
                        red.Dir = new Coor(0, 1);
                        break;
                    case "DownArrow":
                        red.Dir = new Coor(0, -1);
                        break;
                    case "P":
                        //Pause
                        //Console.SetCursorPosition(26, 10);
                        //Console.BackgroundColor = ConsoleColor.Black;
                        //Console.ForegroundColor = ConsoleColor.Yellow;
                        //Console.Write("PAUSA");
                        ////Esperamos a la próxima pulsación de p
                        //string pausa = String.Empty;
                        //while (pausa != "P")
                        //    if (Console.KeyAvailable)
                        //        pausa = Console.ReadKey(true).Key.ToString();
                        break;
                    case "S":
                        //Save
                        //SaveState(t, "save");
                        break;
                    case "Q": // Salir del juego
                        //quit = true;
                        break;
                    case "N":
                        //NextLevel (hack)
                        //next = SKIP; //En vez de if(SKIP) next = true;
                        break;
                }
            }
        }

        private void ReadInputTurn()
        {

        }

        void ProcessInput()
        {

        }

        private void ProcessInputBattle()
        {

        }

        private void ProcessInputTurn()
        {

        }
    }
}
