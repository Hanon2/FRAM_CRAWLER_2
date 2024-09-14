using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Elcometer.Demo.Xamarin.Forms.MVVM
{
    public class ViewFactory : IViewFactory
    {
        private readonly ISimpleContainer _container;
        private readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();

        public ViewFactory(ISimpleContainer container)
        {
            _container = container;
        }

        public void Register<TVM, TV>() where TVM : BaseViewModel where TV : Page
        {
            _map[typeof(TVM)] = typeof(TV);
        }

        public Page Resolve<TVM>(Action<TVM> setStateAction = null) where TVM : BaseViewModel
        {
            TVM viewModel;
            return Resolve(out viewModel, setStateAction);
        }

        public Page Resolve<TVM>(out TVM viewModel, Action<TVM> setStateAction = null) where TVM : BaseViewModel
        {
            viewModel = _container.Resolve<TVM>();

            var viewModelType = typeof(TVM);

            Type viewType;
            if (!_map.TryGetValue(viewModelType, out viewType))
            {
                throw new InvalidOperationException($"Could not find a view type mapped to the view model type {viewModelType.FullName}");
            }

            var view = _container.Resolve(viewType) as Page;

            if (setStateAction != null)
                setStateAction(viewModel);

            view.BindingContext = viewModel;
            return view;
        }

        public Page Resolve<TVM>(TVM viewModel) where TVM : BaseViewModel
        {
            var viewModelType = viewModel.GetType();

            Type viewType;
            if (!_map.TryGetValue(viewModelType, out viewType))
            {
                throw new InvalidOperationException($"Could not a view type mapped to the view model type {viewModelType.FullName}");
            }

            var view = _container.Resolve(viewType) as Page;
            view.BindingContext = viewModel;
            return view;
        }
    }
}