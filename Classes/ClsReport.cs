using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATCPortal
{
    public class ClsReport
    {
        string name, oem;
        int total;

        public ClsReport(string name, string oem, int total)
        {
            this.name = name;
            this.oem = oem;
            this.total = total;
        }
        public string Name {
            get { return name; }
            set { name = value; }
        }

        public string OEM {
            get { return oem; }
            set { oem = value; }
        }

        public int Total {
            get { return total; }
            set { total = value; }
        }
    }
}