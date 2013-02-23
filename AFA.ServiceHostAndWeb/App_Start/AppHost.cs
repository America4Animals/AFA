using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AFA.ServiceHostAndWeb.Helpers;
using AFA.ServiceInterface;
using AFA.ServiceModel;
using ServiceStack.Common.Utils;
using ServiceStack.Configuration;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.FluentValidation;
using ServiceStack.Mvc;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.WebHost.Endpoints;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AFA.ServiceHostAndWeb.App_Start.AppHost), "Start")]

//IMPORTANT: Add the line below to MvcApplication.RegisterRoutes(RouteCollection) in the Global.asax:
//routes.IgnoreRoute("api/{*pathInfo}"); 

/**
 * Entire ServiceStack Starter Template configured with a 'Hello' Web Service and a 'Todo' Rest Service.
 *
 * Auto-Generated Metadata API page at: /metadata
 * See other complete web service examples at: https://github.com/ServiceStack/ServiceStack.Examples
 */

namespace AFA.ServiceHostAndWeb.App_Start
{
    //Hold App wide configuration you want to accessible by your services
    public class AppConfig
    {
        public AppConfig(IResourceManager appSettings)
        {
            this.Env = appSettings.Get("Env", Env.Local);
            this.EnableCdn = appSettings.Get("EnableCdn", false);
            this.CdnPrefix = appSettings.Get("CdnPrefix", "");
            this.AdminUserNames = appSettings.Get("AdminUserNames", new List<string>());
        }

        public Env Env { get; set; }
        public bool EnableCdn { get; set; }
        public string CdnPrefix { get; set; }
        public List<string> AdminUserNames { get; set; }
        public BundleOptions BundleOptions
        {
            get { return Env.In(Env.Local, Env.Dev) ? BundleOptions.Normal : BundleOptions.MinifiedAndCombined; }
        }
    }

    public enum Env
    {
        Local,
        Dev,
        Test,
        Prod,
    }

	public class AppHost
		: AppHostBase
	{		
		public AppHost() //Tell ServiceStack the name and where to find your web services
            : base("StarterTemplate ASP.NET Host", typeof(OrganizationService).Assembly) { }

        public static AppConfig AppConfig;

		public override void Configure(Funq.Container container)
		{
			//Set JSON web services to return idiomatic JSON camelCase properties
			ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;
		
            ////Configure User Defined REST Paths
            //Routes
            //  .Add<Hello>("/hello")
            //  .Add<Hello>("/hello/{Name*}");

			//Uncomment to change the default ServiceStack configuration
			//SetConfig(new EndpointHostConfig {
			//});

            //Register Typed Config some services might need to access
            var appSettings = new AppSettings();
            AppConfig = new AppConfig(appSettings);
            container.Register(AppConfig);

            //Register all your dependencies
            //container.Register(new TodoRepository());
            container.Register<ICacheClient>(new MemoryCacheClient());
		    RegisterServices(container);

            // Register DB
            container.Register<IDbConnectionFactory>(c =>
                                                     new OrmLiteConnectionFactory(
                                                         "~/App_Data/db.sqlite".MapHostAbsolutePath(),
                                                         SqliteOrmLiteDialectProvider.Instance));

            // Reset DB
            using (var resetDb = container.Resolve<ResetDbService>())
            {
                resetDb.Any(null);
            }

            //Enable Authentication
            ConfigureAuth(container);

			//Set MVC to use the same Funq IOC as ServiceStack
			ControllerBuilder.Current.SetControllerFactory(new FunqControllerFactory(container));
		}

		private void ConfigureAuth(Funq.Container container)
		{
			var appSettings = new AppSettings();

			//Default route: /auth/{provider}
			Plugins.Add(new AuthFeature(() => new CustomUserSession(),
				new IAuthProvider[] {
					new CredentialsAuthProvider(appSettings),
                    //new CustomCredentialsAuthProvider(appSettings),
					new FacebookAuthProvider(appSettings), 
					//new TwitterAuthProvider(appSettings), 
					new BasicAuthProvider(appSettings), 
				})); 

			//Default route: /register
			Plugins.Add(new RegistrationFeature());

            //override the default registration validation with your own custom implementation
            container.RegisterAs<CustomRegistrationValidator, IValidator<Registration>>();

            ////Requires ConnectionString configured in Web.Config
            //var connectionString = ConfigurationManager.ConnectionStrings["AppDb"].ConnectionString;
            //container.Register<IDbConnectionFactory>(c =>
            //    new OrmLiteConnectionFactory(connectionString, SqlServerDialect.Provider));

            container.Register<IUserAuthRepository>(c =>
                new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));

            var authRepo = (OrmLiteAuthRepository)container.Resolve<IUserAuthRepository>();
            // Override default regex to allow email addresses as usernames
            authRepo.ValidUserNameRegEx = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
			authRepo.CreateMissingTables();
		}

	    private void RegisterServices(Funq.Container container)
	    {
	        container.Register<IStateProvincesService>(new StateProvincesService());

            container.Register<IOrganizationCategoryService>(new OrganizationCategoryService());
            container.Register<IOrganizationCategoriesService>(new OrganizationCategoriesService());

	        container.Register<IEventCategoriesService>(new EventCategoriesService());

            container.Register<IEventService>(new EventService());
            container.Register<IEventsService>(new EventsService());

	        container.Register<IOrganizationService>(new OrganizationService());
	        container.Register<IOrganizationsService>(new OrganizationsService());

            container.Register<IUserService>(new UserService());
            container.Register<IUsersService>(new UsersService());

            container.Register<ICrueltySpotCategoriesService>(new CrueltySpotCategoriesService());
            container.Register<ICrueltySpotCategoryService>(new CrueltySpotCategoryService());
	    }

	    public static void Start()
		{
			new AppHost().Init();
		}
	}
}