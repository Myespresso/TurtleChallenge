using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingFileReader
{
    public sealed class InvalidSettingException : Exception
    {
       public IReadOnlyList<SettingsError> Errors { get; private set; }
        public InvalidSettingException(List<SettingsError> errors):base("Errors in setting file!")
        {
            this.Errors = errors.AsReadOnly();
        }
    }
}
