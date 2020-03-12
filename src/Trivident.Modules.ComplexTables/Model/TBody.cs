using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.ComplexTables.Model
{
    public class TBody 
    {
        public TBody()
        {
            Rows = new List<Row>();
        }
        public List<Row> Rows { get; set; }
    }
}
