using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHG.Web.Models
{
    public class EmailSystem
    {
        public string port { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public string host { get; set; }
        public bool enableSSL { get; set; }
        public string displayName { get; set; }
    }
}