using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    enum TurnMode { Normal, Plan, Run }

    class Transistor
    {
        TurnMode mode;
        const int LapTime = 20;
        Battlefield field;
        private char currentAttack = ' ';

        public char CurrentAttack
        {
            get => currentAttack;
        }

        public void Run()
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            field = new Battlefield("Transistor2.txt");
            TurnDisplay turnDisplay = new TurnDisplay(field.numRows, field.numCols);
            CaptionDisplay captionDisplay= new CaptionDisplay(field.numRows, field.numCols);
            float turnPercentage = 100f;
            bool playing = true;
            
            int counter = 0;
            mode = TurnMode.Normal;

            field.Show(mode, currentAttack);
            captionDisplay.Show();

            //Bucle principal de juego
            while (playing)
            {
                // input de usuario
                if (counter % field.Red.Speed == 0)
                {
                    if (ReadInput(mode))
                    {
                        field.GetPlayer().Move(mode);
                    } 
                }

                if (mode == TurnMode.Normal) //TODO: Design choice, mode as parameter without if, or without mode but with if
                {
                    field.EnemiesAttack();
                    field.MoveProjectiles();
                    field.MoveEnemies();
                }

                field.Show(mode, CurrentAttack);

                turnDisplay.Show(TurnMode.Normal, turnPercentage, 2, 4, 0, 3);

                if (turnPercentage < 100)
                    turnPercentage += 0.5f;





                // retardo
                System.Threading.Thread.Sleep(LapTime);
                counter++;
            }
        }

        bool ReadInput(TurnMode mode)
        {
            bool dirInput = false;

            if (mode == TurnMode.Normal)
            {
                dirInput= ReadInputBattle();
            }
            else if (mode == TurnMode.Plan)
            {
                dirInput = ReadInputTurn();
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
                        field.GetPlayer().Dir = Coor.LEFT;
                        dirInput = true;
                        break;
                    case "RightArrow":
                        field.GetPlayer().Dir = Coor.RIGHT;
                        dirInput = true;
                        break;
                    case "UpArrow":
                        field.GetPlayer().Dir = Coor.UP;
                        dirInput = true;
                        break;
                    case "DownArrow":
                        field.GetPlayer().Dir = Coor.DOWN;
                        dirInput = true;
                        break;
                    case "D1":
                        field.Red.Attack(mode, 'c');
                        break;
                    case "Spacebar":
                        mode = TurnMode.Plan;
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

        private bool ReadInputTurn()
        {
            bool dirInput = false;

            if (Console.KeyAvailable)
            {
                string tecla = Console.ReadKey().Key.ToString();
                switch (tecla)
                {
                    case "LeftArrow":
                        field.GetPlayer().Dir = Coor.LEFT;
                        dirInput = true;
                        break;
                    case "RightArrow":
                        field.GetPlayer().Dir = Coor.RIGHT;
                        dirInput = true;
                        break;
                    case "UpArrow":
                        field.GetPlayer().Dir = Coor.UP;
                        dirInput = true;
                        break;
                    case "DownArrow":
                        field.GetPlayer().Dir = Coor.DOWN;
                        dirInput = true;
                        break;
                    case "D1":
                        currentAttack = 'c';
                        field.PrintAim(field.Red, CurrentAttack);
                        //field.Red.Attack(mode, 'c');
                        break;
                    case "D2":
                        currentAttack = 'b';
                        field.PrintAim(field.Red, CurrentAttack);
                        //field.Red.Attack(mode, 'b');
                        break;
                    case "D3":
                        currentAttack = 'p';
                        field.PrintAim(field.Red, CurrentAttack);
                        //field.Red.Attack(mode, 'p');
                        break;
                    case "D4":
                        currentAttack = 'l'; //TODO: potentially change to Ping()? (projectile)
                        field.PrintAim(field.Red, CurrentAttack);
                        //field.Red.Attack(mode, 'l');
                        break;
                    case "Enter":
                        field.Red.Attack(mode, currentAttack);
                        break;
                    case "Spacebar":
                        mode = TurnMode.Run;
                        break;
                }
            }

            return dirInput;
        }

        //void ProcessInput()
        //{

        //}

        //private void ProcessInputBattle()
        //{

        //}

        //private void ProcessInputTurn()
        //{

        //}
    }
}
