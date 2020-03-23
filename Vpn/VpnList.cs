using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vpn
{
    public static class VpnList
    {
        public static List<Vpn_> _VpnList = new List<Vpn_>();
    }
    public class Vpn_
    {
        public string IP, User, Pass,Country;        
    }

}
