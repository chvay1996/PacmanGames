using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Brave_new_world
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            Console.CursorVisible = false;
            bool startGame = true;
            int playerX = 0, playerY = 0, playerDX = 0, playerDY = 0, dots = 0, collect = 0;
            bool isPlayer = true;
            int mobX = 0, mobY = 0, mobDX = 0, mobDY = -1;
            char[,] map = MapPoint(out playerX, out mobX, out mobY, out playerY, ref dots);

            DrowMap(map);

            while (startGame)
            {
                Console.SetCursorPosition(playerY, playerX);
                Console.Write('@');

                Console.SetCursorPosition(0, 22);
                Console.WriteLine($"Собрано {collect}, Всего {dots}");

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    PlayGo(key, ref playerDX, ref playerDY);
                }
                if (map[playerX + playerDX, playerY + playerDY] != '#')
                {
                    CollectDots(playerX, playerY, ref collect, map);
                    Move(map, '@', ref playerX, ref playerY, playerDX, playerDY);
                }
                if (map[mobX + mobDX, mobY + mobDY] != '#')
                {
                    Move(map, '$', ref mobX, ref mobY, mobDX, mobDY);
                }
                else
                {
                    PlayGo(random, ref mobDX, ref mobDY);
                }
                if (mobX == playerX && mobY == playerY)
                {
                    startGame = false;
                }

                Thread.Sleep(250);

                if (collect == dots || !isPlayer)
                {
                    isPlayer = false;
                }
            }

            Console.SetCursorPosition(0, 15);

            if (collect == dots)
            {
                Console.WriteLine("Вы победили!)");
            }
            else if (!isPlayer)
            {
                Console.SetCursorPosition(0, 10);
                Console.WriteLine("Вы проиграли! Вас скушали!");
            }
        }

        static char[,] MapPoint(out int playerX, out int mobX, out int mobY, out int playerY, ref int dots)
        {
            playerX = 0; playerY = 0;
            mobX = 0; mobY = 0; ;
            string[] mapFile = {
            "##########################################",
            "#                                    $   #",
            "# ####################### ################",
            "# #          ####     ### ##########@    #",
            "#  # ############### ###  ############  ##",
            "## # ######## ######         ######### ###",
            "## # #####     ######### ############# ###",
            "#                                        #",
            "##########################################"};
                char[,] mapFool = new char[mapFile.Length, mapFile[0].Length];

            for (int i = 0; i < mapFool.GetLength(0); i++)
            {
                for (int j = 0; j < mapFool.GetLength(1); j++)
                {
                    mapFool[i, j] = mapFile[i][j];
                    if (mapFool[i, j] == '@')
                    {
                        playerX = i;
                        playerY = j;
                        mapFool[i, j] = '.';
                    }
                    else if (mapFool[i, j] == '$')
                    {
                        mobX = i;
                        mobY = j;
                        mapFool[i, j] = '.';
                    }
                    else if (mapFool[i, j] == ' ')
                    {
                        mapFool[i, j] = '.';
                        dots++;
                    }
                }
            }
            return mapFool;
        }
        static void DrowMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }
        private static void CollectDots(int playerX, int playerY, ref int collect, char[,] map)
        {
            if (map[playerX, playerY] == '.')
            {
                collect++;
                map[playerX, playerY] = ' ';
            }
        }
        static void Move(char[,] map, char simvol, ref int X, ref int Y, int DX, int DY)
        {
            Console.SetCursorPosition(Y, X);
            Console.Write(map[X, Y]);

            X += DX;
            Y += DY;

            Console.SetCursorPosition(Y, X);
            Console.Write(simvol);

        }
        static void PlayGo(ConsoleKeyInfo key, ref int DX, ref int DY)
        {
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    DX = 0; DY = -1;
                    break;
                case ConsoleKey.UpArrow:
                    DX = -1; DY = 0;
                    break;
                case ConsoleKey.RightArrow:
                    DX = 0; DY = 1;
                    break;
                case ConsoleKey.DownArrow:
                    DX = 1; DY = 0;
                    break;
            }
        }
        static void PlayGo(Random random, ref int DX, ref int DY)
        {
            int goDir = random.Next(1, 5);

            switch (goDir)
            {
                case 1:
                    DX = 0; DY = -1;
                    break;
                case 2:
                    DX = -1; DY = 0;
                    break;
                case 3:
                    DX = 0; DY = 1;
                    break;
                case 4:
                    DX = 1; DY = 0;
                    break;
            }
        }
    }
}
/*Задача:
Сделать игровую карту с помощью двумерного массива. Сделать функцию рисования карты. Помимо этого, дать пользователю возможность перемещаться по карте и 
взаимодействовать с элементами (например пользователь не может пройти сквозь стену)

Все элементы являются обычными символами*/
