using Android.App;
using Android.Runtime;
using Elcometer.Core.Droid;
using Elcometer.Core.Model;
using System;

namespace Elcometer.Demo.Droid
{
    [Application(Theme = "@android:style/Theme.Material.Light")]
    public class DemoApplication : Application
    {
        public DemoApplication(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            ElcometerCore.Instance.Initialise();
        }
    }
}