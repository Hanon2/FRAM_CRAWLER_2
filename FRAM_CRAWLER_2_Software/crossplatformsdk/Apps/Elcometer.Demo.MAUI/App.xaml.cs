using Elcometer.Core;
using Elcometer.Core.Services;
using Elcometer.Demo.Xamarin.Forms.MVVM;
using Elcometer.Demo.Xamarin.Forms.PageModels;
using Elcometer.Demo.Xamarin.Forms.Pages;
using Elcometer.Demo.Xamarin.Forms.Services;
using Microsoft.Extensions.Logging;

#if ANDROID
using triaxis.BluetoothLE;
using Elcometer.Core.Droid.Services;
using Elcometer.Demo.Xamarin.Forms.Droid.Services;
using Elcometer.Demo.MAUI.Platforms.Android.Services;
#endif

#if IOS
using triaxis.BluetoothLE;
using Elcometer.Core.iOS.Services;
using Elcometer.Demo.Xamarin.Forms.iOS.Services;
#endif

namespace Elcometer.Demo.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // as an example of using dependancy injection and MVVM to use the services
            // this demo uses a very basic MVVM framework, but in your app you can use whatever
            // IOC & MVVM framework you wish or just use the singleton "ElcometerCore"

            // below is were the common PCL based services are registered into the dependancy injector
            // there are other in the platform specific services initialised in MainActivity (Android)
            // and AppDelegate (iOS)

            // MVVM singletons

            SimpleContainer container = new SimpleContainer();

            container.RegisterSingleton<ISimpleContainer, SimpleContainer>(container);
            container.RegisterSingleton<IViewFactory, ViewFactory>();
            container.RegisterSingleton<INavigationService, NavigationService>();

            container.RegisterSingleton<IGaugeTypeService, GaugeTypeService>();
            container.RegisterSingleton<IMessagingService, MessagingService>();
            container.RegisterSingleton<IGaugeService, GaugeService>();
            container.RegisterSingleton<ISimpleBatchService, SimpleBatchService>();

#if ANDROID
            container.RegisterSingleton<IMauiHelper, MauiHelper>();
            container.RegisterSingleton<IConnectionService, ConnectionService>();
            container.RegisterSingleton<IPlatformService, PlatformService>();
            container.RegisterSingleton<IBluetoothPickerService, BluetoothPickerService>();
            container.RegisterSingleton<IBluetoothLE, triaxis.BluetoothLE.Platform>(new triaxis.BluetoothLE.Platform(new LoggerFactory()));
#endif

#if IOS
            container.RegisterSingleton<IConnectionService, ConnectionService>();
            container.RegisterSingleton<IPlatformService, PlatformService>();
            container.RegisterSingleton<IBluetoothPickerService, BluetoothPickerService>();
            container.RegisterSingleton<IBluetoothLE, triaxis.BluetoothLE.Platform>(new triaxis.BluetoothLE.Platform(new LoggerFactory()));
#endif

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

            SimpleContainer.Instance = container;

            // link the page models together

            var viewFactory = container.Resolve<IViewFactory>();

            viewFactory.Register<MainPageModel, MainPage>();
            viewFactory.Register<GaugePageModel, GaugePage>();
            viewFactory.Register<BatchesPageModel, BatchesPage>();
            viewFactory.Register<BatchInfoPageModel, BatchInfoPage>();

            MainPage = new SimpleNavigationPage(viewFactory.Resolve<MainPageModel>());
        }
    }
}