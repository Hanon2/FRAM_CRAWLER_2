using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elcometer.Demo.Xamarin.Forms.MVVM
{
    /// <summary>
    /// Simple wrapper around the navigation page that tells the ViewModel of popped pages that they 
    /// the view has been removed
    /// </summary>
    public class SimpleNavigationPage : NavigationPage
    {
        public SimpleNavigationPage() 
        {
            Init();
        }

        public SimpleNavigationPage(Page root) : base(root)
        {
            Init();
        }

        private void Init()
        {
            Popped += SimpleNavigationPage_Popped;
        }

        private void SimpleNavigationPage_Popped(object sender, NavigationEventArgs e)
        {
            var vm = e.Page.BindingContext as BaseViewModel;

            if (vm != null)
            {
                vm.ViewClosed();
            }
        }
    }
}
