
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingFileReader
{
    public struct SettingsError
    {
        public int LineNumber { get; set; }
        public string File { get; set; }
        public string Error { get; set; }
        public override string ToString()
        {
            StringBuilder strb = new StringBuilder();
            strb.Append("Error in :").Append(File).Append("  Line:").Append(LineNumber.ToString()).Append("   ").AppendLine(Error);
            return strb.ToString();
        }
    }
}
