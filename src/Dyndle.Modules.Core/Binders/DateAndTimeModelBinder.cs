using System;
using System.Web.Mvc;

namespace Dyndle.Modules.Core.Binders
{

    /// <summary>
    /// Binder to bind date based on custom format
    /// see http://www.hanselman.com/blog/SplittingDateTimeUnitTestingASPNETMVCCustomModelBinders.aspx
    /// </summary>
    /// <seealso cref="System.Web.Mvc.IModelBinder" />
    public class DateAndTimeModelBinder : IModelBinder
    {
        /// <summary>
        /// Empty constructor to instantiate this class
        /// </summary>
        /// <seealso cref="System.Web.Mvc.IModelBinder" />
        public DateAndTimeModelBinder() { }

        /// <summary>
        /// Binds the model.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">bindingContext</exception>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            //Maybe we're lucky and they just want a DateTime the regular way.
            DateTime? dateTimeAttempt = GetA<DateTime>(bindingContext, "DateTime");
            if (dateTimeAttempt != null)
            {
                return dateTimeAttempt.Value;
            }

            //If they haven't set Month,Day,Year OR Date, set "date" and get ready for an attempt
            if (!this.MonthDayYearSet && !this.DateSet)
            {
                this.Date = "Date";
            }

            //If they haven't set Hour, Minute, Second OR Time, set "time" and get ready for an attempt
            if (!this.HourMinuteSecondSet && !this.TimeSet)
            {
                this.Time = "Time";
            }

            //Did they want the Date *and* Time?
            DateTime? dateAttempt = GetA<DateTime>(bindingContext, this.Date);
            DateTime? timeAttempt = GetA<DateTime>(bindingContext, this.Time);

            //Maybe they wanted the Time via parts
            if (this.HourMinuteSecondSet)
            {
                var hour = GetA<int>(bindingContext, this.Hour);
                var minute = GetA<int>(bindingContext, this.Minute);
                var second = GetA<int>(bindingContext, this.Second);

                if (hour.HasValue && minute.HasValue && second.HasValue)
                {
                    timeAttempt = new DateTime(
                                        DateTime.MinValue.Year, 
                                        DateTime.MinValue.Month, 
                                        DateTime.MinValue.Day,
                                        hour.Value,
                                        minute.Value,
                                        second.Value);
                }
            }

            //Maybe they wanted the Date via parts
            if (this.MonthDayYearSet)
            {
                var year = GetA<int>(bindingContext, this.Year);
                var month = GetA<int>(bindingContext, this.Month);
                var day = GetA<int>(bindingContext, this.Day);

                if (year.HasValue && month.HasValue && day.HasValue)
                {
                    dateAttempt = new DateTime(
                                        year.Value,
                                        month.Value,
                                        day.Value,
                                        DateTime.MinValue.Hour, 
                                        DateTime.MinValue.Minute, 
                                        DateTime.MinValue.Second);
                }
            }

            //If we got both parts, assemble them!
            if (dateAttempt != null && timeAttempt != null)
            {
                return new DateTime(dateAttempt.Value.Year,
                dateAttempt.Value.Month,
                dateAttempt.Value.Day,
                timeAttempt.Value.Hour,
                timeAttempt.Value.Minute,
                timeAttempt.Value.Second);
            }
            //Only got one half? Return as much as we have!
            return dateAttempt ?? timeAttempt;
        }

        private static T? GetA<T>(ModelBindingContext bindingContext, string key) where T : struct
        {
            if (string.IsNullOrEmpty(key)) return null;
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + "." + key);

            //Didn't work? Try without the prefix if needed...
            if (valueResult == null && bindingContext.FallbackToEmptyPrefix)
            {
                valueResult = bindingContext.ValueProvider.GetValue(key);
            }
            return (T?) valueResult?.ConvertTo(typeof(T));
        }


        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        public string Month { get; set; }

        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        /// <value>
        /// The day.
        /// </value>
        public string Day { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets the hour.
        /// </summary>
        /// <value>
        /// The hour.
        /// </value>
        public string Hour { get; set; }

        /// <summary>
        /// Gets or sets the minute.
        /// </summary>
        /// <value>
        /// The minute.
        /// </value>
        public string Minute { get; set; }

        /// <summary>
        /// Gets or sets the second.
        /// </summary>
        /// <value>
        /// The second.
        /// </value>
        public string Second { get; set; }

        /// <summary>
        /// Gets a value indicating whether [date set].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [date set]; otherwise, <c>false</c>.
        /// </value>
        public bool DateSet => !string.IsNullOrEmpty(Date);

        /// <summary>
        /// Gets a value indicating whether [month day year set].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [month day year set]; otherwise, <c>false</c>.
        /// </value>
        public bool MonthDayYearSet => !(string.IsNullOrEmpty(Month) && string.IsNullOrEmpty(Day) && string.IsNullOrEmpty(Year));

        /// <summary>
        /// Gets a value indicating whether [time set].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [time set]; otherwise, <c>false</c>.
        /// </value>
        public bool TimeSet => !string.IsNullOrEmpty(Time);

        /// <summary>
        /// Gets a value indicating whether hour, minute and second values are set.
        /// </summary>
        /// <value>
        ///   <c>true</c> if hour, minute and second values are set; otherwise, <c>false</c>.
        /// </value>
        public bool HourMinuteSecondSet => !(string.IsNullOrEmpty(Hour) && string.IsNullOrEmpty(Minute) && string.IsNullOrEmpty(Second));
    }
}