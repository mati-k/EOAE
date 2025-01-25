using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EOAE_Code.Interfaces
{
    public interface IUseAreaAim
    {
        public float Range { get;}
        public float Radius { get; }
        public string AreaAimPrefab { get; }
    }
}
