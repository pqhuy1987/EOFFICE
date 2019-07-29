using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHG.DataServices.DataClass
{
    public class M_Log : BaseClass
    {
        public string id { get; set; }
        public string loglevel { get; set; }
        public string noidung { get; set; }
        public string ipaddress { get; set; }
        public string mataikhoan { get; set; }
        public string pageurl { get; set; }
        public string referrerurl { get; set; }
        public string ngaytao { get; set; }
        public string limit { get; set; }
        public string offset { get; set; }
    }
}
