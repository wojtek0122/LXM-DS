using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXM_DS.AUTOUPDATE
{
    class AppVersion
    {
        public int _Version;
        public string _Name;
        public string _Comment;
        public string _Date;
        public int _CurrentVersion;

        public AppVersion(int Version, string Name, string Comment, string Date, int CurrentVersion)
        {
            _Version = Version;
            _Name = Name;
            _Comment = Comment;
            _Date = Date;
            _CurrentVersion = CurrentVersion;
        }

    }
}
