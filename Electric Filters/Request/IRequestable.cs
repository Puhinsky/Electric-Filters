using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electric_Filters
{
    public interface IRequestable : IVariable
    {
        public void SetInput(decimal value);
    }
}
