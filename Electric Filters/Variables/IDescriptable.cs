using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electric_Filters.Variables
{
    public interface IDescriptable : IVariable
    {
        public List<IVariable> Description { get; }
    }
}
