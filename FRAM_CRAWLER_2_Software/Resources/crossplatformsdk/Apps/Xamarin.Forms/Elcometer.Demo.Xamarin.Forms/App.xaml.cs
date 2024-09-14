using Elcometer.Core;
using Elcometer.Core.Services;
using Elcometer.Demo.Xamarin.Forms.MVVM;
using Elcometer.Demo.Xamarin.Forms.PageModels;
using Elcometer.Demo.Xamarin.Forms.Pages;

using Xamarin.Forms;

namespace Elcometer.Demo.Xamarin.Forms
{
    public partial class App : Application
    {
        public App(SimpleContainer container)
        {
            InitializeComponent();

            // as an example of using dependancy injection and MVVM to use the services
            // this demo uses a very basic MVVM framework, but in your app you can use whatever
            // IOC & MVVM framework you wish or just use the singleton "ElcometerCore"

            // below is were the common PCL based services are registered into the dependancy injector
            // there are other in the platform specific services initialised in MainActivity (Android)
            // and AppDelegate (iOS)

            // MVVM singletons

            container.RegisterSingleton<ISimpleContainer, SimpleContainer>(container);
            container.RegisterSingleton<IViewFactory, ViewFactory>();
            container.RegisterSingleton<INavigationService, NavigationService>();

            // app singletons

            container.RegisterSingleton<IGaugeTypeService, GaugeTypeService>();
            container.RegisterSingleton<IMessagingService, MessagingService>();
            container.RegisterSingleton<IGaugeService, GaugeService>();
            container.RegisterSingleton<ISimpleBatchService, SimpleBatchService>();

            // register the available batch types that can be downloaded
            ElcometerCoreRegister.RegisterBatches(container.Resolve<ISimpleBatchService>());

            // register the gauges that can be communicated with
            ElcometerCoreRegister.RegisterGaugeTypes(container.Resolve<IGaugeTypeService>());

            // page and page models

            container.RegisterType<MainPageModel>();
            container.RegisterType<MainPage>();
            container.RegisterType<GaugePageModel>();
            container.RegisterType<GaugePage>();
            container.RegisterType<BatchesPageModel>();
            container.RegisterType<BatchesPage>();
            container.RegisterType<BatchInfoPageModel>();
            container.RegisterType<BatchInfoPage>();

            // link the page models together

            var viewFactory = container.Resolve<IViewFactory>();

            viewFactory.Register<MainPageModel, MainPage>();
            viewFactory.Register<GaugePageModel, GaugePage>();
            viewFactory.Register<BatchesPageModel, BatchesPage>();
            viewFactory.Register<BatchInfoPageModel, BatchInfoPage>();

            // show the first page
            MainPage = new SimpleNavigationPage(viewFactory.Resolve<MainPageModel>());
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }
    }
}