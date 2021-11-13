using System;

namespace Battleships
{
    class Program
    {
        static void Main(string[] args)
        {
            int difficulty = 1;
            string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string path1 = homePath + @"\Documents\Games\Battleship\save\playerDisplayBoard.txt";
            string path2 = homePath + @"\Documents\Games\Battleship\save\playerHiddenBoard.txt";
            string path3 = homePath + @"\Documents\Games\Battleship\save\aiDisplayBoard.txt";
            string path4 = homePath + @"\Documents\Games\Battleship\save\aiHiddenBoard.txt";
            string path5 = homePath + @"\Documents\Games\Battleship\save\settings.txt";
            string[] paths = { path1, path2, path3, path4, path5 };

        start:;
            switch (Menus.MainMenu())
            {
                case 0:
                newgame:;
                    difficulty = Menus.SetDifficulty();
                    if (Menus.NewGame(difficulty, paths))
                        goto start;
                    else
                        goto end;

                case 1:
                    if (GameTools.PathExistsAndNotEmpty(paths))
                    {
                        if (Menus.ContinueGame(paths))
                            goto newgame;
                        else
                            goto start;
                    }
                    else
                    {
                        if (Menus.NoSavedGame())
                            goto start;
                        else
                        {
                            if (!Menus.AreYouSure())
                                goto start;
                            else
                                goto end;
                        }
                    }

                case 2:
                    goto start;

                case 3:
                    if (Menus.AreYouSure())
                        goto end;
                    else
                        goto start;

                case -1:
                    Console.Clear();
                    Console.WriteLine("An unexpected problem occured.");
                    goto end;
            }
        end:;
        }
    }
}
