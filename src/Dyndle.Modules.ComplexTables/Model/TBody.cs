using System.Collections.Generic;

namespace Dyndle.Modules.ComplexTables.Model
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
