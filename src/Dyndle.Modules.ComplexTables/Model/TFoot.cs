using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyndle.Modules.ComplexTables.Model
{
    public class TFoot 
    {
        public TFoot()
        {
            Rows = new List<Row>();
        }
        public List<Row> Rows { get; set; }
    }
}
