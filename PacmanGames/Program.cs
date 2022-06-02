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
            GameProgress();
        }

        static void GameProgress()
        {
            Console.CursorVisible = false;
            bool startGame = true;
            int characterCoordinatesX;
            int characterCoordinatesY;
            int characterDirectionX = 0;
            int characterDirectionY = 0;
            int dots = 0;
            int collect = 0;
            int positionOutputGameResult = 15;
            int GameDelay = 250;
            char[,] map = MapPoint(out characterCoordinatesX, out characterCoordinatesY, ref dots);

            DrowMap(map);

            while (startGame)
            {
                Console.SetCursorPosition(characterCoordinatesY, characterCoordinatesX);
                Console.Write('@');

                Console.SetCursorPosition(0, 22);
                Console.WriteLine($"Собрано {collect}, Всего {dots + 1}");

                ArrowControl(ref characterDirectionX, ref characterDirectionY);
                WalkingThroughWalls(map, ref characterCoordinatesX, ref characterCoordinatesY, ref collect, characterDirectionX, characterDirectionY);

                Thread.Sleep(GameDelay);

                EndGame(ref positionOutputGameResult, ref collect, ref dots, ref startGame);
            }
        }

        static void WalkingThroughWalls(char[,] map, ref int characterCoordinatesX, ref int characterCoordinatesY, ref int collect, int characterDirectionX, int characterDirectionY)
        {
            if (map[characterCoordinatesX + characterDirectionX, characterCoordinatesY + characterDirectionY] != '#')
            {
                CollectDots(characterCoordinatesX, characterCoordinatesY, ref collect, map);
                Move(map, '@', ref characterCoordinatesX, ref characterCoordinatesY, characterDirectionX, characterDirectionY);
            }
        }
        static bool EndGame(ref int positionOutputGameResult, ref int collect, ref int dots, ref bool startGame)
        {
            Console.SetCursorPosition(0, positionOutputGameResult);

            if (collect == dots)
            {
                Console.WriteLine("Вы победили!)");
                startGame = false;
            }

            return startGame;
        }

        static char[,] MapPoint(out int characterCoordinatesX, out int characterCoordinatesY, ref int dots)
        {
            characterCoordinatesX = 0; 
            characterCoordinatesY = 0;
            string[] mapFile = {
            "##########################################",
            "#                                        #",
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
                        characterCoordinatesX = i;
                        characterCoordinatesY = j;
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

        private static void CollectDots(int characterCoordinatesX, int characterCoordinatesY, ref int collect, char[,] map)
        {
            if (map[characterCoordinatesX, characterCoordinatesY] == '.')
            {
                collect++;
                map[characterCoordinatesX, characterCoordinatesY] = ' ';
            }
        }

        static void Move(char[,] map, char simvol, ref int coordinatesX, ref int coordinatesY, int directionX, int directionY)
        {
            Console.SetCursorPosition(coordinatesY, coordinatesX);
            Console.Write(map[coordinatesX, coordinatesY]);

            coordinatesX += directionX;
            coordinatesY += directionY;

            Console.SetCursorPosition(coordinatesY, coordinatesX);
            Console.Write(simvol);

        }

        static void ArrowControl(ref int directionX, ref int directionY)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        directionX = 0; directionY = -1;
                        break;
                    case ConsoleKey.UpArrow:
                        directionX = -1; directionY = 0;
                        break;
                    case ConsoleKey.RightArrow:
                        directionX = 0; directionY = 1;
                        break;
                    case ConsoleKey.DownArrow:
                        directionX = 1; directionY = 0;
                        break;
                }
            }
        }
    }
}
/*Задача:
Сделать игровую карту с помощью двумерного массива. Сделать функцию рисования карты. Помимо этого, дать пользователю возможность перемещаться по карте и 
взаимодействовать с элементами (например пользователь не может пройти сквозь стену)

Все элементы являются обычными символами*/
