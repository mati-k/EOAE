using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOAE_Code.Interfaces
{
    public interface IGetDataList<T>
    {
        public List<T> GetDataList();
    }
}
