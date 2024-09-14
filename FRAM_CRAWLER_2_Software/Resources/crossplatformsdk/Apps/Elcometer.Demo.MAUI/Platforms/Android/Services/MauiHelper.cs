using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elcometer.Demo.MAUI.Platforms.Android.Services
{
    public class MauiHelper : IMauiHelper
    {
        private MainActivity _activity;

        public MauiHelper()
        {
        }

        public void SetMainActivity(MainActivity activity)
        {
            _activity = activity;
        }

        public MainActivity GetMainActivity()
        {
            return _activity;
        }
    }
}
