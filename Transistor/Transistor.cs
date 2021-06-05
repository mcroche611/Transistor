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

            field = new Battlefield("Transistor4.txt");
            TurnDisplay turnDisplay = new TurnDisplay(field.numRows, field.numCols);
            CaptionDisplay captionDisplay= new CaptionDisplay(field.numRows, field.numCols);
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
                    if (ReadInput())
                    {
                        field.GetPlayer().Move(mode);
                    }
                }

                if (mode == TurnMode.Normal) //TODO: Design choice, mode as parameter without if, or without mode but with if
                {
                    field.EnemiesAttack();
                }

                field.MoveProjectiles(mode);
                field.MoveEnemies(mode);

                field.Show(mode, CurrentAttack);
                float lifePercentage = field.Red.Life;
                turnDisplay.Show(mode, field.TurnPercentage, lifePercentage,  1, 2, 3, 4);

                /// TEST ////
                if (lifePercentage < 50)
                    turnDisplay.CrashEnabled = false;
                /// FIN TEST ////

                if (mode == TurnMode.Normal)
                {
                    if (field.TurnPercentage < 100) 
                        field.TurnPercentage += 0.5f;
                }

                // retardo
                System.Threading.Thread.Sleep(LapTime);
                counter++;
            }
        }

        bool ReadInput()
        {
            bool dirInput = false;

            if (mode == TurnMode.Normal)
            {
                dirInput= ReadInputNormal();
            }
            else if (mode == TurnMode.Plan)
            {
                dirInput = ReadInputTurn();
            }
            else
            {
                char c = field.Red.GetActionTurn();
                
                if (c != '\0')
                {
                    dirInput = ReadInputRun(c);
                }
                else
                {
                    mode = TurnMode.Normal;
                }
            }

            return dirInput;
        }

        private bool ReadInputNormal()
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
                        field.Red.PosTurn = field.Red.Pos;
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
            //Limpiamos el buffer de teclado
            while (Console.KeyAvailable)
                Console.ReadKey(false);

            return dirInput;
        }

        private bool ReadInputTurn()
        {
            bool dirInput = false;

            if (Console.KeyAvailable)
            {
                string key = Console.ReadKey().Key.ToString();

                if (field.TurnPercentage > 0)
                {
                    switch (key)
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
                            break;
                        case "D2":
                            currentAttack = 'b';
                            break;
                        case "D3":
                            currentAttack = 'p';
                            break;
                        case "D4":
                            currentAttack = 'l';
                            break;
                    }
                }

                switch (key)
                { 
                    case "Enter":
                        field.PrintAim(field.Red, currentAttack);
                        field.Red.Attack(mode, currentAttack);
                        currentAttack = ' ';
                        break;
                    case "Spacebar":
                        field.Red.Pos = field.Red.PosTurn;
                        field.TurnPercentage = 100;
                        mode = TurnMode.Run;
                        break;
                }
            }

            return dirInput;
        }

        private bool ReadInputRun(char action)
        {
            bool dirInput = false;

            if (field.TurnPercentage > 0)
            {
                switch (action)
                {
                    case 'i':
                        field.GetPlayer().Dir = Coor.LEFT;
                        dirInput = true;
                        break;
                    case 'r':
                        field.GetPlayer().Dir = Coor.RIGHT;
                        dirInput = true;
                        break;
                    case 'u':
                        field.GetPlayer().Dir = Coor.UP;
                        dirInput = true;
                        break;
                    case 'd':
                        field.GetPlayer().Dir = Coor.DOWN;
                        dirInput = true;
                        break;
                    case 'c':
                        field.Red.Attack(mode, action);
                        break;
                    case 'b':
                        field.Red.Attack(mode, action);
                        break;
                    case 'p':
                        field.Red.Attack(mode, action);
                        currentAttack = ' ';
                        break;
                    case 'l':
                        field.Red.Attack(mode, action);
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
