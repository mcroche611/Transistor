using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Transistor
{
    class Menu
    {
        public enum Action { Continue, Start, Controls, Credits, Exit };

        Transistor t;

        public Menu(Transistor transistor)
        {
            t = transistor;
        }

        public int RunMenu(out string profile, ref bool quit)
        {
            int level = 0;
            bool levelSelected = false;
            profile = null;

            while (!levelSelected)
            {
                Console.Clear();

                Menu.Action input = SelectAction(out level);

                switch (input)
                {
                    case Menu.Action.Continue: //Juego
                        if (File.Exists("profiles")) //No deja seleccionar Continue si no hay ningún profile preexistente
                        {
                            Console.Clear();
                            profile = AskName();
                            level = LastLevelPlayed(profile) + 1;

                            if (level == -1)
                            {
                                Console.WriteLine("Profile not found, starting new game");
                                Console.ReadLine();
                                level = 0;
                            }

                            levelSelected = true;
                        }
                        break;

                    case Action.Start:
                        Console.Clear();
                        profile = AskName();
                        levelSelected = true;
                        break;

                    case Menu.Action.Controls: //Se quieren ver los controles del juego
                        ShowControlsScreen();
                        break;

                    case Action.Credits:
                        ShowCreditsScreen();
                        break;

                    case Menu.Action.Exit: //Termina la ejecución
                        quit = true;
                        levelSelected = true;
                        break;
                }
            }

            return level;
        }

        //Selecciona una acción del menú principal
        public Action SelectAction(out int level)
        {
            level = 0;

            //Menú principal:

            ShowTitleScreen();

            int pointer = (int)Action.Continue;
            int[] pos = { 36, 36, 36, 36, 36 };
            string[] label = { " Load Game ", " New Game ", " Controls ", " Credits ", " Exit " }; 

            ButtonSelect(ref pointer, pos, label);

            //Si ha elegido continuar la partida
            if ((Action)pointer == Action.Continue)
            {
                Console.Clear();
            }

            return (Action)pointer;
        }

        // Muestra los botones seleccionados del menú
        private bool ButtonSelect(ref int pointer, int[] pos, string[] label)
        {
            bool actionSelected = false;

            do
            {
                //Escribe botones
                for (int i = 0; i < label.Length; i++)
                {
                    if (i != 0 || (i == 0 && File.Exists("profiles"))) //No se escribe el primer botón si no hay ningún Profile guardado
                    {
                        if (pointer == i)
                        {
                            Console.BackgroundColor = ConsoleColor.Cyan;
                            Console.ForegroundColor = ConsoleColor.Black;

                            Console.SetCursorPosition(pos[i], i + 8);
                            Console.WriteLine(label[i]);

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        }
                        else
                        {
                            Console.SetCursorPosition(pos[i], i + 8);
                            Console.WriteLine(label[i]);
                        }
                    }
                }

                ProcessInputMenu(ReadInputMenu(), ref pointer, ref actionSelected, 4);
            } while (!actionSelected);

            return actionSelected;
        }

        private char ReadInputMenu()
        {
            char d = ' ';
            while (d == ' ')
            {
                if (Console.KeyAvailable)
                {
                    string tecla = Console.ReadKey(true).Key.ToString();
                    switch (tecla)
                    {
                        case "UpArrow":
                            d = 'u';
                            break;
                        case "DownArrow":
                            d = 'd';
                            break;
                        case "Enter":
                            d = 'e';
                            break;
                    }
                }
            }
            return d;
        }

        private void ProcessInputMenu(char c, ref int cursor, ref bool enter, int numOptions)
        {
            switch (c)
            {
                case 'u':
                    if (cursor > 0) cursor--;
                    break;
                case 'd':
                    if (cursor < numOptions) cursor++;
                    break;
                case 'e':
                    enter = true;
                    break;
            }
        }

        private void ShowTitleScreen()
        {
            //Pantalla de titulo
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;
            ShowScreen(40, 25);

            Console.SetCursorPosition(0, 4);
            Console.WriteLine("                                    Transistor");
            Console.WriteLine("                  From Supergiant Games, adapted by Marta Croche Trigo");
        }

        private void ShowScreen(int x, int y)
        {
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                {
                    Console.SetCursorPosition(2 * i, j);
                    Console.Write("  ");
                }
        }

        // Pregunta el nombre del profile de la partida
        public string AskName()
        {
            string userName="";
            while (userName == null || userName == "")
            {
                Console.Clear();
                Console.CursorVisible = true;
                Console.SetCursorPosition(10, 5);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("User: ");
                Console.CursorVisible = false;
                userName = Console.ReadLine();
            }
            return userName;
        }

        public void ShowControlsScreen()
        {
            string text = ReadFile("Controls.txt");

            Console.Clear();

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine(text);
            Console.WriteLine();
            Console.WriteLine("                 Press any key to continue");
            Console.ReadLine();
        }

        public void ShowCreditsScreen()
        {
            string text = ReadFile("Credits.txt");

            Console.Clear();

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine(text);
            Console.WriteLine();
            Console.WriteLine("                         Press any key to continue");
            Console.ReadLine();
        }

        public int LastLevelPlayed(string nickname)
        {
            try
            {
                if (IsProfile(nickname, out string linea)) //Si hay un profile con ese nickname
                    return int.Parse(linea.Remove(0, nickname.Length + 1).TrimStart());
                else return -1;
            }
            catch (Exception e)
            {
                return -1; //Si no es un nickname válido
            }
        }

        // Determina si hay un profile con ese nombre
        static bool IsProfile(string nickname, out string linea)
        {
            bool encontrado = false;
            linea = string.Empty;

            if (File.Exists("profiles"))
            {
                StreamReader entrada = new StreamReader("profiles");

                while (!encontrado && !entrada.EndOfStream)
                {
                    linea = entrada.ReadLine().ToLower();
                    encontrado = linea.StartsWith(nickname.ToLower());
                }

                entrada.Close();
            }
            return encontrado;
        }

        // Guarda el último nivel superado por un jugador
        public void SaveData(int nivel, string nickname)
        {
            string data = string.Empty;
            nickname = nickname.ToLower();

            if (nivel <= t.MAXLEVEL)
            {
                if (File.Exists("profiles"))
                {
                    //Guardamos todos los datos que no sean los del usuario actual
                    StreamReader entrada = new StreamReader("profiles");

                    while (!entrada.EndOfStream)
                    {
                        string linea = entrada.ReadLine().ToLower();

                        if (!linea.StartsWith(nickname))
                            data += linea + Environment.NewLine;
                    }

                    entrada.Close();
                }

                //Escribimos todos los datos de nuevo sin los del usuario actual
                StreamWriter salida = new StreamWriter("profiles", false);

                salida.Write(data);

                //Escribimos los datos del usuario actual en minúsculas
                salida.WriteLine(nickname.ToLower() + ": " + nivel);

                salida.Close();
            }
        }

        public string ReadFile(string fileName)
        {
            string content = ""; 

            if (File.Exists(fileName))
            {
                StreamReader file = new StreamReader(fileName);

                content = file.ReadToEnd();

                file.Close();
            }

            return content;
        }
    }
}
