using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trevo.API.Utility
{
   public class ModelStateErrorMessage
    {
       public static List<string> ValidationError(System.Web.Http.ModelBinding.ModelStateDictionary ModelState)
       {
           List<string> errors = new List<string>();
           foreach (var state in ModelState)
           {
               foreach (var error in state.Value.Errors)
               {
                   errors.Add(error.ErrorMessage);
               }
           }
           return errors;
       }
    }
}
