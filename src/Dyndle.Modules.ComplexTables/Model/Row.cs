using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyndle.Modules.ComplexTables.Model
{
    public enum RowType { Header, Normal, Total }
    public class Row 
    {
        public Row()
        {
            Styling = new Styling();
            RowType = RowType.Normal;
            Cells = new List<Cell>();
        }
        public RowType RowType { get; set; }
        public IList<Cell> Cells { get; set; }
        public Styling Styling { get; set; }
    }
}
