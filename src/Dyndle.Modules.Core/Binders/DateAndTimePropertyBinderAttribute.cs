using System.Web.Mvc;

namespace Dyndle.Modules.Core.Binders
{
    /// <summary>
    /// Class DateAndTimePropertyBinderAttribute.
    /// Implements the <see cref="Dyndle.Modules.Core.Binders.PropertyBinderAttribute" />
    /// </summary>
    /// <seealso cref="Dyndle.Modules.Core.Binders.PropertyBinderAttribute" />
    public class DateAndTimePropertyBinderAttribute : PropertyBinderAttribute
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public string Date { get; set; }
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>The month.</value>
        public string Month { get; set; }
        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        /// <value>The day.</value>
        public string Day { get; set; }
        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets the hour.
        /// </summary>
        /// <value>The hour.</value>
        public string Hour { get; set; }
        /// <summary>
        /// Gets or sets the minute.
        /// </summary>
        /// <value>The minute.</value>
        public string Minute { get; set; }
        /// <summary>
        /// Gets or sets the second.
        /// </summary>
        /// <value>The second.</value>
        public string Second { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateAndTimePropertyBinderAttribute"/> class.
        /// </summary>
        public DateAndTimePropertyBinderAttribute() : base(typeof(DateAndTimeModelBinder)) { }

        /// <summary>
        /// Creates the binder.
        /// </summary>
        /// <returns>IModelBinder.</returns>
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
