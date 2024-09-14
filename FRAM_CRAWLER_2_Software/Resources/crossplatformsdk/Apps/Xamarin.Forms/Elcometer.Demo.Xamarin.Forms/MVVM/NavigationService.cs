using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Elcometer.Demo.Xamarin.Forms.MVVM
{
    public class NavigationService : INavigationService
    {
        private readonly IViewFactory _viewFactory;

        public NavigationService(IViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        private INavigation Navigation
        {
            get
            {
                var page = GetCurrentPage(Application.Current.MainPage);
                return page.Navigation;
            }
        }

        public async Task<BaseViewModel> PopAsync()
        {
            var previousViewModel = GetCurrentViewModel(Navigation);
            var nextViewModel = GetPreviousViewModel(Navigation);
            
            await Navigation.PopAsync().ConfigureAwait(false);

            if (nextViewModel != null)
                nextViewModel.ViewEntered();

            if (previousViewModel != null)
            {
                previousViewModel.ViewLeaved();
                // the previousViewModel.ViewClosed is triggered by the popped event of the navigationPage
            }

            return nextViewModel;
        }

        public async Task<T> PushAsync<T>(Action<T> setStateAction = null) where T : BaseViewModel
        {
            var previousViewModel = GetCurrentViewModel(Navigation);

            T nextViewModel;
            var nextView = _viewFactory.Resolve(out nextViewModel, setStateAction);
            
            await Navigation.PushAsync(nextView).ConfigureAwait(false);

            nextViewModel.ViewOpened();
            nextViewModel.ViewEntered();

            if (previousViewModel != null)
                previousViewModel.ViewLeaved();

            return nextViewModel;
        }

        public async Task<T> PushAsync<T>(T viewModel) where T : BaseViewModel
        {
            var nextViewModel = viewModel;
            var nextView = _viewFactory.Resolve(nextViewModel);
            var previousViewModel = GetCurrentViewModel(Navigation);
            
            await Navigation.PushAsync(nextView);

            nextViewModel.ViewOpened();
            nextViewModel.ViewEntered();

            if (previousViewModel != null)
                previousViewModel.ViewLeaved();

            return nextViewModel;
        }

        private Page GetCurrentPage(Page rootPage)
        {
            var page = rootPage;
            bool containsAnotherPage;

            do
            {
                containsAnotherPage = true;

                if (page is MasterDetailPage)
                {
                    page = ((MasterDetailPage)page).Detail;
                }
                else if (page is IPageContainer<Page>)
                {
                    page = ((IPageContainer<Page>)page).CurrentPage;
                }
                else
                {
                    containsAnotherPage = false;
                }
            } while (containsAnotherPage);

            return page;
        }

        private Page GetCurrentView(INavigation navigation)
        {
            return navigation?.NavigationStack?.LastOrDefault();
        }

        private BaseViewModel GetCurrentViewModel(INavigation navigation)
        {
            var currentView = GetCurrentView(navigation);
            return currentView?.BindingContext as BaseViewModel;
        }

        private Page GetPreviousView(INavigation navigation)
        {
            if (navigation != null && navigation.NavigationStack != null && navigation.NavigationStack.Count > 1)
            {
                return navigation.NavigationStack[navigation.NavigationStack.Count - 2];
            }

            return null;
        }

        private BaseViewModel GetPreviousViewModel(INavigation navigation)
        {
            var previousView = GetPreviousView(navigation);
            return previousView?.BindingContext as BaseViewModel;
        }
    }
}