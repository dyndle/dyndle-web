using System.Collections.Generic;

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
