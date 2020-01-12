using Engine;
using SettingFileReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Console
{
    partial class Program
    {
        static IGame game;
        static void Main(string[] args)
        {



            BoardSetting board = null;
            MovesSetting moves = null;
            try
            {
                board = GetBoardSetting();
                moves = GetMovesSetting();

            }
            catch (InvalidSettingException ex)
            {
                foreach (var item in ex.Errors)
                {
                    WriteError(item.ToString());
                }


            }

            game = null;
            if (board != null && moves != null)
            {
                try
                {
                    game = Game
                       .Create(board.BoardWithd, board.BoardHeight)
                       .LayMine(board.Mines)
                       .SetExitPoint(board.ExitPoint)
                       .AddTurtle(board.StartPoint, board.Direction, G_TurtleStatusChanged);
                }
                catch (Exception ex)
                {
                    WriteError(ex.Message);

                }
            }
            if (game != null)
            {
                Play(moves);
            }
            else
            {
                WriteError("Failur in running application!");
            }
            System.Console.ReadLine();
        }

        private static void Play(MovesSetting moves)
        {
            foreach (var mv in moves.Moves)
            {
                GameIsFinished = false;

                game.Reset();
                processResultDesc = "In Danger";
                bool Error = false; ;
                foreach (char chr in mv)
                {
                    if (GameIsFinished )
                    {
                        break;
                    }

                    if (chr == 'r')
                    {
                        try
                        {
                            System.Console.ForegroundColor = ConsoleColor.Green;
                            game.Rotate();
                            
                        }
                        catch (Exception ex) { WriteError(" => Error: " + ex.Message); Error = true; break; }
                    }
                    else if (chr == 'm')
                    {
                        try
                        {
                            System.Console.ForegroundColor = ConsoleColor.Yellow;
                            game.Move();
                        }
                        catch (Exception ex) { WriteError(" => Error: "+ex.Message); Error = true; break; }
                    }
                    System.Console.Write(chr);
                }
                
                System.Console.ForegroundColor = ConsoleColor.White;
                if (!Error)
                {
                    System.Console.WriteLine(" =>   " + processResultDesc);
                }
            }
        }

        static string processResultDesc;
        static bool GameIsFinished;
        private static void G_TurtleStatusChanged(object sender, TurtleStatusChangedEventArg e)
        {
            if (e.Status == TurtleState.Escaped)
            {
                processResultDesc = "Escaped";
                GameIsFinished = true;
            }
            if (e.Status == TurtleState.Killed)
            {
                processResultDesc = "Killed";
                GameIsFinished = true;


            }
        }
    }
}
