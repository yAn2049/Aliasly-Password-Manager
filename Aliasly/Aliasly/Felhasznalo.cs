using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliasly
{
    public class Felhasznalo
    {
        public int FelhasznaloId { get; set; }
        public string Nev { get; set; }
        public string EMail { get; set; }
        public string Url { get; set; }
        public string Hozzafuzes { get; set; }
        public int JelszoId { get; set; }
    }
}
