using System.Collections.Generic;

namespace Dyndle.Modules.ComplexTables.Model
{
    public class THead 
    {
        public THead()
        {
            Rows = new List<Row>();
        }
        public List<Row> Rows { get; set; }
    }
}
