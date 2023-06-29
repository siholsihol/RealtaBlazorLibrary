using System.Globalization;

namespace BlazorMenu.Services
{
    public class CultureInfoBuilder
    {
        //number
        private string NumberDecimalSeparator = ",";
        private string NumberGroupSeparator = ".";
        private int NumberDecimalDigits = 2;
        private NumberFormatInfo NumberFormatInfo = default;

        //date
        private string LongDatePattern = "MMMM d, yyyy";
        private string ShortDatePattern = "M/d/yy";
        private string LongTimePattern = "hh:mm:ss tt";
        private string ShortTimePattern = "hh:mm tt";
        private DateTimeFormatInfo DateTimeFormatInfo = default;

        public CultureInfoBuilder()
        {
            NumberFormatInfo = Thread.CurrentThread.CurrentUICulture.NumberFormat;
            DateTimeFormatInfo = Thread.CurrentThread.CurrentUICulture.DateTimeFormat;
        }

        #region Number
        public CultureInfoBuilder WithNumberDecimalSeparator(string pcDecimalSeparator)
        {
            NumberDecimalSeparator = pcDecimalSeparator;
            NumberGroupSeparator = pcDecimalSeparator == "," ? NumberGroupSeparator : ",";

            return this;
        }

        public CultureInfoBuilder WithNumberDecimalDigits(int piNumberDecimalDigits)
        {
            NumberDecimalDigits = piNumberDecimalDigits;

            return this;
        }

        public CultureInfoBuilder WithNumberFormatInfo(string pcDecimalSeparator, int piNumberDecimalDigits)
        {
            NumberDecimalSeparator = pcDecimalSeparator;
            NumberGroupSeparator = pcDecimalSeparator == "," ? NumberGroupSeparator : ",";

            NumberDecimalDigits = piNumberDecimalDigits;

            NumberFormatInfo.NumberDecimalSeparator = NumberDecimalSeparator;
            NumberFormatInfo.NumberGroupSeparator = NumberGroupSeparator;
            NumberFormatInfo.NumberDecimalDigits = NumberDecimalDigits;

            return this;
        }
        #endregion

        #region Date
        public CultureInfoBuilder WithLongDatePattern(string pcLongDatePattern)
        {
            LongDatePattern = pcLongDatePattern;

            return this;
        }

        public CultureInfoBuilder WithShortDatePattern(string pcShortDatePattern)
        {
            ShortDatePattern = pcShortDatePattern;

            return this;
        }

        public CultureInfoBuilder WithLongTimePattern(string pcLongTimePattern)
        {
            LongTimePattern = pcLongTimePattern;

            return this;
        }

        public CultureInfoBuilder WithShortTimePattern(string pcShortTimePattern)
        {
            ShortTimePattern = pcShortTimePattern;

            return this;
        }

        public CultureInfoBuilder WithDatePattern(string pcLongDatePattern, string pcShortDatePattern)
        {
            LongDatePattern = pcLongDatePattern;
            ShortDatePattern = pcShortDatePattern;

            DateTimeFormatInfo.LongDatePattern = LongDatePattern;
            DateTimeFormatInfo.ShortDatePattern = ShortDatePattern;

            return this;
        }

        public CultureInfoBuilder WithTimePattern(string pcLongTimePattern, string pcShortTimePattern)
        {
            LongTimePattern = pcLongTimePattern;
            ShortTimePattern = pcShortTimePattern;

            DateTimeFormatInfo.LongTimePattern = LongTimePattern;
            DateTimeFormatInfo.ShortTimePattern = ShortTimePattern;

            return this;
        }
        #endregion

        public CultureInfo BuildCultureInfo()
        {
            var loCulture = Thread.CurrentThread.CurrentUICulture;
            loCulture.NumberFormat = NumberFormatInfo;
            loCulture.DateTimeFormat = DateTimeFormatInfo;

            return loCulture;
        }
    }
}
