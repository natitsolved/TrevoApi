
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using System.Web.Http;
using System.Web.Http.Cors;
using Trevo.Core.Data;
using Trevo.Data;
using Trevo.Services.Chat;
using Trevo.Services.Country;
using Trevo.Services.FavouriteService;
using Trevo.Services.HobbyService;
using Trevo.Services.Language;
using Trevo.Services.MomentService;
using Trevo.Services.TransliterationService;
using Trevo.Services.UserBlockService;
using Trevo.Services.UserFollowService;
using Trevo.Services.Users;
using Trevo.Services.UserUploadService;

namespace Trevo.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType(typeof(IRepository<>), typeof(EfRepository<>));

            container.RegisterType<IDbContext, ObjectContext>(new HierarchicalLifetimeManager());

            container.RegisterType<IUserService, UserService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMomentsService, MomentsService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICountryService, CountryService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILanguageLevelService, LanguageLevelService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILanguageService, LanguageService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserLanguageService, UserLanguageService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserUploadsService, UserUploadsService>(new HierarchicalLifetimeManager());
            container.RegisterType<IChatOfflineService, ChatOfflineService>(new HierarchicalLifetimeManager());
            container.RegisterType<IHobbiesService, HobbiesService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFavouritesService, FavouritesService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserFollowDetailService, UserFollowDetailService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBlockService, BlockService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserTransliterationService, UserTransliterationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITransliterationService, TransliterationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<IdentityUser>, UserStore<IdentityUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<IdentityUser>>(new HierarchicalLifetimeManager());

            //container.RegisterType<IRoleStore<IdentityRole>, RoleStore<IdentityRole>>(new HierarchicalLifetimeManager());

            container.RegisterType<IRoleStore<IdentityRole, string>,
    RoleStore<IdentityRole, string, IdentityUserRole>>(new HierarchicalLifetimeManager());
            //        container.RegisterType<RoleManager<IdentityRole>>(new HierarchicalLifetimeManager());
            //container.RegisterType<IRoleStore<IdentityRole>, RoleStore<IdentityRole>>(new HierarchicalLifetimeManager());
            container.RegisterType<RoleManager<IdentityRole>>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);


            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;

            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.Routes.MapHttpRoute(
               name: "DefaultApi1",
               routeTemplate: "api/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );
            //config.EnableSystemDiagnosticsTracing();
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
        }
    }
}
