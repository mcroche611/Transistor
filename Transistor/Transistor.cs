using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Transistor
{
    enum TurnMode { Normal, Plan, Run }
    enum Input { Changed, Same, Undone };

    class Transistor
    {
        TurnMode mode;
        Input dirInput;
        const int LapTime = 20;
        bool quit = false;
        bool goToMenu = false;
        Battlefield field;
        public int MAXLEVEL = 9;
        SoundFX fx = new SoundFX();

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

                Menu menu = new Menu(this);
                fx.PlayMenuIntro();
                int level = menu.RunMenu(out string profile, ref quit);


                while (!quit && level <= MAXLEVEL && !goToMenu)
                {
                    goToMenu = false;
                    fx.PlayNewLevel();
                    Console.Clear();
                    field = new Battlefield("Transistor" + level + ".txt");

                    TurnDisplay turnDisplay = new TurnDisplay(field, field.NumRows, field.NumCols);
                    CaptionDisplay captionDisplay = new CaptionDisplay(0, field.NumCols, level);

                    int lapCounter = 0;
                    mode = TurnMode.Normal;

                    field.Show(mode, currentAttack);
                    captionDisplay.Show();

                    //Bucle principal de juego
                    //La partida continúa hasta que no queden enemigos o no queden vidas
                    while (field.EnemyList.Count() > 0 && field.Red.Life > 0 && !quit && !goToMenu) 
                    {
                        // input de usuario
                        if (lapCounter % field.Red.Speed == 0)
                        {
                            dirInput = ReadInput();

                            if (dirInput == Input.Changed) //Mueve al jugador
                            {
                                field.Red.Move(mode);
                            }
                            else if (dirInput == Input.Undone) //Deshace un movimiento
                            {
                                field.Red.UndoMove();
                            }
                        }

                        if (mode == TurnMode.Normal)
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
                                field.TurnPercentage += 2f;
                        }

                        for (int i = 0; i < field.Red.AttacksCoolDown.Length; i++) //modificar coolDowns del jugador
                        {
                            if (field.Red.AttacksCoolDown[i] > 0)
                            {
                                field.Red.AttacksCoolDown[i]--;
                            }
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
                            fx.PlayGameOver();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("GAME OVER");
                            Console.WriteLine();
                            Console.WriteLine("             Press M to exit to menu, Q to exit game, and any key to retry");
                        }
                        else // Nivel completado
                        {
                            fx.PlayLevelCompleted();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("LEVEL CLEARED");

                            menu.SaveData(level, profile);
                            level++;

                            Console.WriteLine();
                            Console.WriteLine("             Press M to exit to menu, Q to exit game, and any key to continue to next level");

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

                if (!quit && !goToMenu)
                {
                    fx.PlayOutro();

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("         CONGRATULATIONS!!! GAME COMPLETE! CLOUDBANK IS NOW FREE");

                    menu.SaveData(level, profile);

                    Console.WriteLine();
                    Console.WriteLine("         Press M to exit to menu, Q to exit game");

                    string answer2 = Console.ReadLine().ToLower();
                    if (answer2 == "m")
                    {
                        goToMenu = true;
                    }
                    else if (answer2 == "q")
                    {
                        quit = true;
                    }
                }
            }
        }

        //Recibe el input de cada modo de juego
        private Input ReadInput()
        {
            Input dirInput = Input.Same;

            if (mode == TurnMode.Normal)
            {
                dirInput = ReadInputNormal();
            }
            else if (mode == TurnMode.Plan)
            {
                dirInput = ReadInputTurn();
            }
            else //En Run, pasa al ReadInput la acción a realizar
            {
                char c = field.Red.GetActionTurn();

                if (c != '\0')
                {
                    dirInput = ReadInputRun(c);
                }
                else //Al acabar el string de movimientos, vuelve al modo Normal
                {
                    mode = TurnMode.Normal;
                }
            }

            return dirInput;
        }

        private Input ReadInputNormal()
        {
            Input dirInput = Input.Same;

            if (Console.KeyAvailable)
            {
                string tecla = Console.ReadKey().Key.ToString();
                switch (tecla)
                {
                    case "LeftArrow":
                        field.Red.Dir = Coor.LEFT;
                        dirInput = Input.Changed;
                        break;
                    case "RightArrow":
                        field.Red.Dir = Coor.RIGHT;
                        dirInput = Input.Changed;
                        break;
                    case "UpArrow":
                        field.Red.Dir = Coor.UP;
                        dirInput = Input.Changed;
                        break;
                    case "DownArrow":
                        field.Red.Dir = Coor.DOWN;
                        dirInput = Input.Changed;
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
                            fx.PlayTurn1();
                            // Guarda la posición y la dirección de comienzo del Turn
                            field.Red.PosTurn = field.Red.Pos;
                            field.Red.DirTurn = field.Red.Dir;
                            mode = TurnMode.Plan;
                        }
                        break;
                    case "Escape": // Salir del juego
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

        private Input ReadInputTurn()
        {
            Input dirInput = Input.Same;

            if (Console.KeyAvailable)
            {
                string key = Console.ReadKey().Key.ToString();

                if (field.TurnPercentage > 0)
                {
                    switch (key)
                    {
                        case "LeftArrow":
                            field.Red.Dir = Coor.LEFT;
                            dirInput = Input.Changed;
                            break;
                        case "RightArrow":
                            field.Red.Dir = Coor.RIGHT;
                            dirInput = Input.Changed;
                            break;
                        case "UpArrow":
                            field.Red.Dir = Coor.UP;
                            dirInput = Input.Changed;
                            break;
                        case "DownArrow":
                            field.Red.Dir = Coor.DOWN;
                            dirInput = Input.Changed;
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
                        // Deja planeado el ataque correspondiente
                        field.PrintAim(currentAttack);
                        field.Red.Attack(mode, currentAttack);
                        // Deselecciona el ataque
                        currentAttack = ' ';
                        break;
                    case "Spacebar":
                        fx.PlayTurn2();
                        // Restaura la posición y la dirección de comienzo del Turn
                        field.Red.Pos = field.Red.PosTurn;
                        field.Red.Dir = field.Red.DirTurn;

                        field.TurnPercentage = 100;
                        mode = TurnMode.Run;
                        currentAttack = ' ';
                        break;
                    case "Backspace": //Deshace la última acción planeada
                        dirInput = field.Red.UndoAction();
                        break;
                    case "Escape": // Salir del juego
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

        private Input ReadInputRun(char action)
        {
            Input dirInput = Input.Same;

            if (field.TurnPercentage > 0)
            {
                switch (action)
                {
                    case 'i':
                        field.Red.Dir = Coor.LEFT;
                        dirInput = Input.Changed;
                        break;
                    case 'r':
                        field.Red.Dir = Coor.RIGHT;
                        dirInput = Input.Changed;
                        break;
                    case 'u':
                        field.Red.Dir = Coor.UP;
                        dirInput = Input.Changed;
                        break;
                    case 'd':
                        field.Red.Dir = Coor.DOWN;
                        dirInput = Input.Changed;
                        break;
                    case 'c':
                        field.Red.Attack(mode, action);
                        break;
                    case 'b':
                        field.Red.Attack(mode, action);
                        break;
                    case 'p':
                        field.Red.Attack(mode, action);
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
