using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Couchbase;

namespace MenuzRus {

    public class CastleControllerFactory : DefaultControllerFactory {

        #region Public Properties

        public IWindsorContainer Container { get; private set; }

        #endregion Public Properties

        #region Constructors

        public CastleControllerFactory() {
            this.Container = new WindsorContainer();
            this.Container.Kernel.Resolver.AddSubResolver(new ArrayResolver(this.Container.Kernel));

            this.Container.Register(Types.FromAssembly(Assembly.GetExecutingAssembly()).BasedOn<IController>().Configure(c => c.LifeStyle.Is(LifestyleType.Transient)));
            this.Container.Register(Component.For<IHttpContextProvider>().LifeStyle.Singleton.ImplementedBy(typeof(HttpContextProvider)));
            this.Container.Register(Component.For<ISessionData>().LifeStyle.PerWebRequest.ImplementedBy(typeof(SessionData)));

            //Services
            this.Container.Register(Component.For<Services.IAlertService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(Services.AlertService)));
            this.Container.Register(Component.For<MenuzRus.ICategoryService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(MenuzRus.CategoryService)));
            this.Container.Register(Component.For<MenuzRus.ICommentService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(MenuzRus.CommentService)));
            this.Container.Register(Component.For<MenuzRus.IConfirmationService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(MenuzRus.ConfirmationService)));
            this.Container.Register(Component.For<MenuzRus.ICustomerService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(MenuzRus.CustomerService)));
            this.Container.Register(Component.For<MenuzRus.IFloorService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(MenuzRus.FloorService)));
            this.Container.Register(Component.For<MenuzRus.IInventoryService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(MenuzRus.InventoryService)));
            this.Container.Register(Component.For<MenuzRus.IItemService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(MenuzRus.ItemService)));
            this.Container.Register(Component.For<MenuzRus.IItemProductService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(MenuzRus.ItemProductService)));
            this.Container.Register(Component.For<Services.ILogService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(Services.ILogService)));
            this.Container.Register(Component.For<MenuzRus.ILoginService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(MenuzRus.LoginService)));
            this.Container.Register(Component.For<MenuzRus.IMenuService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(MenuzRus.MenuService)));
            this.Container.Register(Component.For<MenuzRus.IOrderService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(MenuzRus.OrderService)));
            this.Container.Register(Component.For<Services.IReportsService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(Services.ReportsService)));
            this.Container.Register(Component.For<MenuzRus.ISettingsService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(MenuzRus.SettingsService)));
            this.Container.Register(Component.For<MenuzRus.IUserService>().LifeStyle.PerWebRequest.ImplementedBy(typeof(MenuzRus.UserService)));
        }

        #endregion Constructors

        #region Overrides

        public override IController CreateController(RequestContext requestContext, String controllerName) {
            IController controller = base.CreateController(requestContext, controllerName);

            return controller;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType) {
            try {
                if (controllerType == null) return null;
                return this.Container.Resolve(controllerType) as IController;
            }
            catch (Exception ex) {
                return null;
            }
        }

        #endregion Overrides
    }
}