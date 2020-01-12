using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingFileReader
{
    public sealed class MovesSetting
    {
        string _File;
        public MovesSetting(string fileName)
        {
            _Moves = new List<string>();
            _File = fileName;
        }
        List<string> _Moves;

        public IReadOnlyList<string> Moves { get { return _Moves.AsReadOnly(); } }


        public void Load()
        {
            var data = File.ReadAllLines(this._File);
            string temp = string.Empty;
            List<SettingsError> errors = new List<SettingsError>();
            int lineNo = 1;
            foreach (var item in data)
            {

                if (!string.IsNullOrWhiteSpace(item))
                {
                    temp = item.Replace(",", "");

                    if (temp.Any(x => x != 'r' && x != 'm'))
                    {
                        errors.Add(new SettingsError { LineNumber = lineNo, Error = "Invalid character found, Valid characters are 'r' and 'm' ", File = _File });
                    }
                    else
                    {
                        _Moves.Add(temp);
                    }
                }
                lineNo++;
            }

            if (errors.Any())
            {
                throw new InvalidSettingException(errors);
            }

        }


    }
}
