using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliasly.MVVM.Model
{
    public class Adatok
    {
        public int Id { get; set; }
        public string Jelszo { get; set; }
        public string EMail { get; set; }
        public string Nev { get; set; }
        public string Url { get; set; }
        public string Hozzafuzes { get; set; }
        public int MesterId { get; set; }
    }
}
