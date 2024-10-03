using Elcometer.Core.iOS.Services;
using Elcometer.Core.Services;
using Elcometer.Demo.Xamarin.Forms.iOS.Services;
using Elcometer.Demo.Xamarin.Forms.MVVM;
using Elcometer.Demo.Xamarin.Forms.Services;
using Foundation;
using Microsoft.Extensions.Logging;
using triaxis.BluetoothLE;
using UIKit;

namespace Elcometer.Demo.Xamarin.Forms.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // as an example of using dependancy injection and MVVM to use the services
            // this demo uses a simple MVVM framework, but in your app you can use whatever
            // IOC & MVVM framework you wish or just use the singleton "ElcometerCore"
            //
            // these are the platform dependant services registered into the dependacy injector
            var container = new SimpleContainer();
            container.RegisterSingleton<IConnectionService, ConnectionService>();
            container.RegisterSingleton<IPlatformService, PlatformService>();
            container.RegisterSingleton<IBluetoothPickerService, BluetoothPickerService>();
            container.RegisterSingleton<IBluetoothLE, triaxis.BluetoothLE.Platform>(new triaxis.BluetoothLE.Platform(new LoggerFactory()));

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App(container));

            return base.FinishedLaunching(app, options);
        }
    }
}