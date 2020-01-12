using Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingFileReader
{
    public  sealed class BoardSetting
    {
        string _File;
        public BoardSetting(string filePath)
        {
            _File = filePath;
            _Mines = new List<Point>();
        }
        public int BoardHeight { get; private set; }
        public int BoardWithd { get; private set; }
        public Point StartPoint { get; private set; }
        public Point ExitPoint { get; private set; }
        List<Point> _Mines;
        public IReadOnlyList<Point> Mines { get { return _Mines.AsReadOnly(); } }
        public Direction Direction { get; set; }
        public void Load()
        {
            var data = File.ReadAllLines(this._File);
            string temp = string.Empty;
            List<SettingsError> errors = new List<SettingsError>();

            var err = FillHeigtAndWidth(data);
            if (err.HasValue)
            {
                errors.Add(err.Value);
            }
            err = FillStartPoint(data);
            if (err.HasValue)
            {
                errors.Add(err.Value);
            }
            err = FillExitPoint(data);
            if (err.HasValue)
            {
                errors.Add(err.Value);
            }
            var errs = FillMines(data);
            if (errs.Any())
            {
                errors.AddRange(errs);
            }
            if (errors.Any())
            {
                throw new InvalidSettingException(errors);
            }
        }


        private List<SettingsError> FillMines(string[] data)
        {
            List<SettingsError> errors = new List<SettingsError>();

            for (int i = 3; i < data.Length; i++)
            {
                try
                {
                    var minePointStrings = data[i].Split(',');
                    var mx = int.Parse(minePointStrings[2]);
                    var my = int.Parse(minePointStrings[4]);
                    _Mines.Add(new Point { X = mx, Y = my });
                }

                catch
                {
                    var err = new SettingsError { LineNumber = i + 1, File = this._File, Error = "Error in Mine setting Valid format is : Mine, x,#, y,#" };
                    errors.Add(err);
                }
            }
            return errors;

        }

        private SettingsError? FillExitPoint(string[] data)
        {
            SettingsError? err = null;

            try
            {
                var exitPointStrings = data[2].Split(',');
                var ex=int.Parse(exitPointStrings[2]);
                var ey=int.Parse(exitPointStrings[4]);
                ExitPoint = new Point { X = ex, Y = ey };
            }
            catch
            {
                err = new SettingsError { LineNumber = 3, File = this._File, Error = "Error in exit point setting Valid format is : Exit point, x,#, y,#" };
            }
            return err;
        }

        private SettingsError? FillStartPoint(string[] data)
        {
            SettingsError? err = null;

            try
            {
                var startPositionStrings = data[1].Split(',');
                var sx =int.Parse(startPositionStrings[2]);
                var sy=int.Parse(startPositionStrings[4]);
                StartPoint = new Point { X = sx, Y = sy };

                var dir = startPositionStrings[6].Trim();
                if (dir.ToLower() == Direction.North.ToString().ToLower())
                {
                    Direction = Direction.North;
                }
                else if (dir.ToLower() == Direction.West.ToString().ToLower())
                {
                    Direction = Direction.West;
                }
                else if (dir.ToLower() == Direction.East.ToString().ToLower())
                {
                    Direction = Direction.East;
                }
                else if (dir.ToLower() == Direction.South.ToString().ToLower())
                {
                    Direction = Direction.South;
                }
                else
                {
                    throw new Exception("Invalid direction");
                }
            }
            catch
            {
                err = new SettingsError { LineNumber = 2, File = this._File, Error = "Error in start point setting Valid format is : Starting position, x,# , y,# , dir,North,South,East,West" };
            }
            return err;
        }

        private SettingsError? FillHeigtAndWidth(string[] data)
        {
            SettingsError? err = null;
            try
            {


                var sizeStrings = data[0].Split(',');
                var w=int.Parse(sizeStrings[1] );
                this.BoardWithd = w;
                var h=int.Parse(sizeStrings[2] );
                this.BoardHeight = h;
            }
            catch
            {
                err = new SettingsError { LineNumber = 1, File = this._File, Error = "Error in board size setting Valid format is : Size,#,#" };
            }
            return err;
        }
    }
}
