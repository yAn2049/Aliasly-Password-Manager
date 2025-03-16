using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliasly.MVVM.Model
{
    public class MesterKulcs
    {
        public int MesterId { get; set; }
        public string KulcsString { get; set; }
        public string SaltString { get; set; }
        public string HashedKulcs { get; set; }
    }
}
