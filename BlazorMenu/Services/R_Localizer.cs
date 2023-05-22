using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_BlazorFrontEnd.Interfaces;
using R_ContextFrontEnd;
using System.Globalization;

namespace BlazorMenu.Services
{
    public class R_Localizer<T> : R_Localizer, R_ILocalizer<T>
    {
        public R_Localizer(R_ContextHeader contextHeader) : base(contextHeader)
        {
        }

        public string this[string pcResourceId]
        {
            get
            {
                return base.GetStringFromResource(typeof(T), pcResourceId);
            }
        }
    }

    public class R_Localizer : R_ILocalizer
    {
        private readonly R_ContextHeader _contextHeader;

        public string this[Type poType, string pcResourceId, CultureInfo poCulture = null, string pcResourceName = ""]
        {
            get
            {
                return GetStringFromResource(poType, pcResourceId, poCulture, pcResourceName);
            }
        }

        public R_Localizer(R_ContextHeader contextHeader)
        {
            _contextHeader = contextHeader;
        }

        protected string GetStringFromResource(Type poType, string pcResourceId, CultureInfo poCulture = null, string pcResourceName = "")
        {
            string lcMessage = "";

            try
            {
                lcMessage = R_FrontUtility.R_GetMessage(poType, pcResourceId, poCulture, pcResourceName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lcMessage = "[Error for MsgId=" + pcResourceId + "] - " + ex.Message;
            }

            return lcMessage;
        }

        public R_Error R_GetError(Type poType, string pcResourceId, CultureInfo poCulture = null, string pcResourceName = "")
        {
            R_Error loError = null;

            try
            {
                loError = R_FrontUtility.R_GetError(poType, pcResourceId, poCulture, pcResourceName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return loError;
        }

        public R_ErrorDetail R_GetErrorDetail(Type poType, string pcResourceId, CultureInfo poCulture = null, string pcResourceName = "")
        {
            R_ErrorDetail loErrorDetail = null;

            try
            {
                loErrorDetail = R_FrontUtility.R_GetErrorDetail(poType, pcResourceId, poCulture, pcResourceName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return loErrorDetail;
        }
    }
}
