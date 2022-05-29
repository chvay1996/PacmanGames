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
            Random random = new Random();
            Console.CursorVisible = false;
            bool startGame = true;
            int characterCoordinatesX = 0;
            int characterCoordinatesY = 0;
            int CharacterDirectionX = 0;
            int CharacterDirectionY = 0;
            int dots = 0;
            int collect = 0;
            int coordinatesEnemyX = 0;
            int coordinatesEnemyY = 0;
            int directionEnemyX = 0;
            int directionEnemyY = -1;
            int positionOutputGameResult = 15;
            int GameDelay = 250;
            bool isPlayer = true;
            char[,] map = MapPoint(out characterCoordinatesX, out coordinatesEnemyX, out coordinatesEnemyY, out characterCoordinatesY, ref dots);

            DrowMap(map);

            while (startGame)
            {
                Console.SetCursorPosition(characterCoordinatesY, characterCoordinatesX);
                Console.Write('@');

                Console.SetCursorPosition(0, 22);
                Console.WriteLine($"Собрано {collect}, Всего {dots}");

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    ArrowControl(key, ref CharacterDirectionX, ref CharacterDirectionY);
                }
                if (map[characterCoordinatesX + CharacterDirectionX, characterCoordinatesY + CharacterDirectionY] != '#')
                {
                    CollectDots(characterCoordinatesX, characterCoordinatesY, ref collect, map);
                    Move(map, '@', ref characterCoordinatesX, ref characterCoordinatesY, CharacterDirectionX, CharacterDirectionY);
                }
                if (map[coordinatesEnemyX + directionEnemyX, coordinatesEnemyY + directionEnemyY] != '#')
                {
                    Move(map, '$', ref coordinatesEnemyX, ref coordinatesEnemyY, directionEnemyX, directionEnemyY);
                }
                else
                {
                    ArrowControl(random, ref directionEnemyX, ref directionEnemyY);
                }
                if (coordinatesEnemyX == characterCoordinatesX && coordinatesEnemyY == characterCoordinatesY)
                {
                    startGame = false;
                }

                Thread.Sleep(GameDelay);

                if (collect == dots || !isPlayer)
                {
                    isPlayer = false;
                }
            }
            EndGame(ref positionOutputGameResult, ref collect, ref dots, ref isPlayer);
        }

        static void EndGame(ref int positionOutputGameResult, ref int collect, ref int dots, ref bool isPlayer)
        {
            Console.SetCursorPosition(0, positionOutputGameResult);

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

        static char[,] MapPoint(out int characterCoordinatesX, out int coordinatesEnemyX, out int coordinatesEnemyY, out int characterCoordinatesY, ref int dots)
        {
            characterCoordinatesX = 0; 
            characterCoordinatesY = 0;
            coordinatesEnemyX = 0; 
            coordinatesEnemyY = 0; ;
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
                        characterCoordinatesX = i;
                        characterCoordinatesY = j;
                        mapFool[i, j] = '.';
                    }
                    else if (mapFool[i, j] == '$')
                    {
                        coordinatesEnemyX = i;
                        coordinatesEnemyY = j;
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

        static void ArrowControl(ConsoleKeyInfo key, ref int directionX, ref int directionY)
        {
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

        static void ArrowControl(Random random, ref int directionX, ref int directionY)
        {
            byte maxDirectionEnemy = 5;
            int goDir = random.Next(1, maxDirectionEnemy);

            switch (goDir)
            {
                case 1:
                    directionX = 0; directionY = -1;
                    break;
                case 2:
                    directionX = -1; directionY = 0;
                    break;
                case 3:
                    directionX = 0; directionY = 1;
                    break;
                case 4:
                    directionX = 1; directionY = 0;
                    break;
            }
        }
    }
}
/*Задача:
Сделать игровую карту с помощью двумерного массива. Сделать функцию рисования карты. Помимо этого, дать пользователю возможность перемещаться по карте и 
взаимодействовать с элементами (например пользователь не может пройти сквозь стену)

Все элементы являются обычными символами*/
