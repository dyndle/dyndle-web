using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.ComplexTables.Model
{
    public class Table
    {
        public Table()
        {
            Styling = new Styling();
        }
        public string SheetName { get; set; }
        public THead THead { get; set; }
        public TBody TBody { get; set; }
        public TFoot TFoot { get; set; }
        public Styling Styling { get; set; }
    }
}
