using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOAE_Code.Interfaces
{
    public interface IDataManager<T>
    {
        public void Add(T item);
    }
}
