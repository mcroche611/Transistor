using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Transistor
{
    enum TurnMode { Normal, Plan, Run }

    class Transistor
    {
        TurnMode mode;
        const int LapTime = 20;
        const int MAXLEVEL = 10;
        bool quit = false;
        bool goToMenu = false;
        Battlefield field;

        //enum Action { Controls, Continue, Start, Credits, Exit };
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

            while (!quit)
            {
                quit = false;
                goToMenu = false;

                Menu menu = new Menu();

                int level = menu.RunMenu(out string profile, ref quit);

                Console.Clear();

                while (!quit && level < MAXLEVEL && !goToMenu)
                {
                    goToMenu = false;

                    field = new Battlefield("Transistor" + level + ".txt");
                    TurnDisplay turnDisplay = new TurnDisplay(field, field.numRows, field.numCols);
                    CaptionDisplay captionDisplay = new CaptionDisplay(field.numRows, field.numCols);
                    //bool playing = true;

                    int lapCounter = 0;
                    mode = TurnMode.Normal;

                    field.Show(mode, currentAttack);
                    captionDisplay.Show();

                    //Bucle principal de juego
                    while (field.EnemyList.Count() > 0 && field.Red.Life > 0 && !quit && !goToMenu) //La partida continúa hasta que no queden enemigos o no queden vidas
                    {
                        // input de usuario
                        if (lapCounter % field.Red.Speed == 0)
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
                        field.MoveEnemies(lapCounter, mode);

                        field.Show(mode, CurrentAttack);
                        float lifePercentage = field.Red.Life;
                        turnDisplay.Show(mode, field.TurnPercentage, lifePercentage);

                        if (mode == TurnMode.Normal) //Aumentar gradualmente la barra de Turn
                        {
                            if (field.TurnPercentage < 100)
                                field.TurnPercentage += 0.5f;
                        }

                        // retardo
                        System.Threading.Thread.Sleep(LapTime);
                        lapCounter++;
                    }

                    // Resultado
                    Console.Clear();
                    Console.SetCursorPosition(20, 5);

                    if (!quit && !goToMenu)
                    {
                        //Limpiamos el buffer de teclado
                        while (Console.KeyAvailable)
                            Console.ReadKey(false);

                        Console.BackgroundColor = ConsoleColor.Black;

                        if (field.Red.Life <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("GAME OVER");
                            Console.WriteLine();
                            Console.WriteLine("         Press M to exit to menu, Q to exit game, and any key to retry");
                        }
                        else // Nivel completado
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("LEVEL CLEARED");

                            menu.SaveData(level, profile);
                            level++;

                            Console.WriteLine();
                            Console.WriteLine("         Press M to exit to menu, Q to exit game, and any key to continue to next level");

                        }
                        string answer = Console.ReadLine().ToLower();
                        if (answer == "m")
                        {
                            goToMenu = true;
                        }
                        else if (answer == "q")
                        {
                            quit = true;
                        }
                    }
                }
            }

            bool ReadInput()
            {
                bool dirInput = false;

                if (mode == TurnMode.Normal)
                {
                    dirInput = ReadInputNormal();
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
        }

        bool ReadInputNormal()
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
                        if (field.Red.AttacksEnabled[0])
                            field.Red.Attack(mode, 'c');
                        break;
                    case "D2":
                        if (field.Red.AttacksEnabled[1])
                            field.Red.Attack(mode, 'b');
                        break;
                    case "D3":
                        if (field.Red.AttacksEnabled[2])
                            field.Red.Attack(mode, 'p');
                        break;
                    case "D4":
                        if (field.Red.AttacksEnabled[3])
                            field.Red.Attack(mode, 'l');
                        break;
                    case "Spacebar":
                        if (field.TurnPercentage >= 100)
                        {
                            field.Red.PosTurn = field.Red.Pos;
                            mode = TurnMode.Plan;
                        }
                        break;
                    case "Q": // Salir del juego
                        quit = true;
                        break;
                    case "M": // Salir al menú
                        goToMenu = true;
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

        bool ReadInputTurn()
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
                            if (field.Red.AttacksEnabled[0])
                                currentAttack = 'c';
                            break;
                        case "D2":
                            if (field.Red.AttacksEnabled[1])
                                currentAttack = 'b';
                            break;
                        case "D3":
                            if (field.Red.AttacksEnabled[2])
                                currentAttack = 'p';
                            break;
                        case "D4":
                            if (field.Red.AttacksEnabled[3])
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
                        currentAttack = ' ';
                        break;
                    case "Q": // Salir del juego
                        quit = true;
                        break;
                    case "M": // Salir al menú
                        goToMenu = true;
                        break;
                }
            }
            //Limpiamos el buffer de teclado
            while (Console.KeyAvailable)
                Console.ReadKey(false);

            return dirInput;
        }

        bool ReadInputRun(char action)
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
    }
}
