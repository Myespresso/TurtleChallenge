using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettingFileReader;

namespace UI.Console
{
    partial class Program
    {
        private static MovesSetting GetMovesSetting()
        {
            var defaultFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "moves.txt");

            var path = GetFilePath(defaultFile, "Move");
            MovesSetting setting = null;
            if (path != null)
            {
                setting = new MovesSetting(path); ;
                setting.Load();
            }
            return setting;
        }
        private static BoardSetting GetBoardSetting()
        {
            var defaultFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "board.txt");

            var path = GetFilePath(defaultFile, "Board Setting");
            BoardSetting setting = null;
            if (path != null)
            {
                setting = new BoardSetting(path); ;
                setting.Load();
            }
            return setting;
        }
        private static void WriteError(string msg)
        {
            var oldColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(msg);
            System.Console.ForegroundColor = oldColor;
        }
        private static string GetFilePath(string defaultFile, string fileType)
        {
            System.Console.Write($"Enter {fileType} File path, Or Press enter to use {defaultFile}:");
            var path = System.Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path))
            {
                path = defaultFile;
            }
            if (!File.Exists(path))
            {
                WriteError($"File {path} does not exist!");
                path = null;
            }
            else
            {

                path = Path.Combine(path);
            }
            return path;
        }

    }
}
