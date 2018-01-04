using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Trevo.Services.Users;

namespace Trevo.API.ActionFilters
{
    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        private const string Token = "Token";
      

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            //  Get API key provider
            try
            {
                var provider = filterContext.ControllerContext.Configuration
                .DependencyResolver.GetService(typeof(IUserService)) as IUserService;

                if (filterContext.Request.Headers.Contains(Token))
                {
                    var tokenValue = filterContext.Request.Headers.GetValues(Token).First();


                    // Validate Token
                    //if (provider != null && !provider.ValidateToken(tokenValue))
                    //{
                    //    var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Request" };
                    //    filterContext.Response = responseMessage;
                    //}
                }
                else
                {
                    filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
            }
            catch (System.Exception)
            {

                var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Request" };
                filterContext.Response = responseMessage;
            }
            

            base.OnActionExecuting(filterContext);

        }
    }
}