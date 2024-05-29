using System.Globalization;

namespace BlazorMenu.Services
{
    public class CultureInfoBuilder
    {
        //number
        private string _numberDecimalSeparator = ",";
        private string _numberGroupSeparator = ".";
        private int _numberDecimalDigits = 2;
        private NumberFormatInfo _numberFormatInfo = default;

        //date
        private string _longDatePattern = "MMMM d, yyyy";
        private string _shortDatePattern = "M/d/yy";
        private string _longTimePattern = "hh:mm:ss tt";
        private string _shortTimePattern = "hh:mm tt";
        private DateTimeFormatInfo _dateTimeFormatInfo = default;

        public CultureInfoBuilder()
        {
            _numberFormatInfo = Thread.CurrentThread.CurrentUICulture.NumberFormat;
            _dateTimeFormatInfo = Thread.CurrentThread.CurrentUICulture.DateTimeFormat;
        }

        #region Number
        public CultureInfoBuilder WithNumberDecimalSeparator(string pcDecimalSeparator)
        {
            _numberDecimalSeparator = pcDecimalSeparator;
            _numberGroupSeparator = pcDecimalSeparator == "," ? _numberGroupSeparator : ",";

            return this;
        }

        public CultureInfoBuilder WithNumberDecimalDigits(int piNumberDecimalDigits)
        {
            _numberDecimalDigits = piNumberDecimalDigits;

            return this;
        }

        public CultureInfoBuilder WithNumberFormatInfo(string pcDecimalSeparator, int piNumberDecimalDigits)
        {
            _numberDecimalSeparator = pcDecimalSeparator;
            _numberGroupSeparator = pcDecimalSeparator == "," ? _numberGroupSeparator : ",";

            _numberDecimalDigits = piNumberDecimalDigits;

            _numberFormatInfo.NumberDecimalSeparator = _numberDecimalSeparator;
            _numberFormatInfo.NumberGroupSeparator = _numberGroupSeparator;
            _numberFormatInfo.NumberDecimalDigits = _numberDecimalDigits;

            return this;
        }
        #endregion

        #region Date
        public CultureInfoBuilder WithLongDatePattern(string pcLongDatePattern)
        {
            _longDatePattern = pcLongDatePattern;

            return this;
        }

        public CultureInfoBuilder WithShortDatePattern(string pcShortDatePattern)
        {
            _shortDatePattern = pcShortDatePattern;

            return this;
        }

        public CultureInfoBuilder WithLongTimePattern(string pcLongTimePattern)
        {
            _longTimePattern = pcLongTimePattern;

            return this;
        }

        public CultureInfoBuilder WithShortTimePattern(string pcShortTimePattern)
        {
            _shortTimePattern = pcShortTimePattern;

            return this;
        }

        public CultureInfoBuilder WithDatePattern(string pcLongDatePattern, string pcShortDatePattern)
        {
            _longDatePattern = pcLongDatePattern;
            _shortDatePattern = pcShortDatePattern;

            _dateTimeFormatInfo.LongDatePattern = _longDatePattern;
            _dateTimeFormatInfo.ShortDatePattern = _shortDatePattern;

            return this;
        }

        public CultureInfoBuilder WithTimePattern(string pcLongTimePattern, string pcShortTimePattern)
        {
            _longTimePattern = pcLongTimePattern;
            _shortTimePattern = pcShortTimePattern;

            _dateTimeFormatInfo.LongTimePattern = _longTimePattern;
            _dateTimeFormatInfo.ShortTimePattern = _shortTimePattern;

            return this;
        }
        #endregion

        public CultureInfo BuildCultureInfo()
        {
            var loCulture = Thread.CurrentThread.CurrentUICulture;
            loCulture.NumberFormat = _numberFormatInfo;
            loCulture.DateTimeFormat = _dateTimeFormatInfo;

            return loCulture;
        }
    }
}
