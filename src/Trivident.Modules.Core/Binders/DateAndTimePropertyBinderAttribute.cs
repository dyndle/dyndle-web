using Trivident.Modules.Authentication.Binders;
using System.Web.Mvc;

namespace Trivident.Modules.Core.Binders
{
    public class DateAndTimePropertyBinderAttribute : PropertyBinderAttribute
    {
        public string Date { get; set; }
        public string Time { get; set; }

        public string Month { get; set; }
        public string Day { get; set; }
        public string Year { get; set; }

        public string Hour { get; set; }
        public string Minute { get; set; }
        public string Second { get; set; }

        public DateAndTimePropertyBinderAttribute() : base(typeof(DateAndTimeModelBinder)) { }

        public override IModelBinder CreateBinder()
        {
            return new DateAndTimeModelBinder()
            {
                Date = Date,
                Time = Time,
                Month = Month,
                Day = Day,
                Year = Year,
                Hour = Hour,
                Minute = Minute,
                Second = Second
            };
        }
    }
}
