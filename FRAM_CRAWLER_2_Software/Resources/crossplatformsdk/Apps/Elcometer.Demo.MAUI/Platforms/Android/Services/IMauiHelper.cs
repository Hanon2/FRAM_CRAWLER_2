using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elcometer.Demo.MAUI.Platforms.Android.Services
{
    public interface IMauiHelper
    {
        void SetMainActivity(MainActivity activity);

        MainActivity GetMainActivity();
    }
}
