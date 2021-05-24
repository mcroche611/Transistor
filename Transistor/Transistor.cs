using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    enum TurnMode { normal, plan, run }

    class Transistor
    {
        TurnMode mode;

        Battlefield field;

        public void Run()
        {
            Console.CursorVisible = false;

            field = new Battlefield("Transistor.txt");

            bool playing = true;
            int lap = 200;
            mode = TurnMode.normal;

            //Bucle principal de juego
            while (playing)
            {
                // input de usuario
                if (ReadInput(mode))
                    field.GetPlayer().Move(mode);
                field.Show(mode);
                // retardo
                System.Threading.Thread.Sleep(lap);
            }
        }

        bool ReadInput(TurnMode mode)
        {
            bool dirInput = false;

            if (mode == TurnMode.normal)
            {
                dirInput= ReadInputBattle();
            }

            return dirInput;
        }

        private bool ReadInputBattle()
        {
            bool dirInput = false;

            if (Console.KeyAvailable)
            {
                string tecla = Console.ReadKey().Key.ToString();
                switch (tecla)
                {
                    case "LeftArrow":
                        field.GetPlayer().Dir = new Coor(0, -1);
                        dirInput = true;
                        break;
                    case "RightArrow":
                        field.GetPlayer().Dir = new Coor(0, 1);
                        dirInput = true;
                        break;
                    case "UpArrow":
                        field.GetPlayer().Dir = new Coor(-1, 0);
                        dirInput = true;
                        break;
                    case "DownArrow":
                        field.GetPlayer().Dir = new Coor(1, 0);
                        dirInput = true;
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

            return dirInput;
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
