using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace Trevo.API.Helper
{
    public class ErrorHelper
    {
        /// <summary>
        /// Get Errors from Model State
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static List<string> GetErrorListFromModelState
                                              (ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return errorList;
        }

        /// <summary>
        /// checks if the password is valid
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>

        public static bool IsValidPassword(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                if (password.Length < 6)
                    return false;
                if (password.Any(c => char.IsDigit(c)))
                {
                    if (password.Any(c => char.IsUpper(c)))
                    {
                        return true;
                    }
                }
                else

                    return false;
            }
            else
                return false;

            return false;
        }
    }
}