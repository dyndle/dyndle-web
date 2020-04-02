namespace Dyndle.Modules.ComplexTables.Model
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
