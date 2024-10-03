using System;
using Xamarin.Forms;

namespace Elcometer.Demo.Xamarin.Forms.MVVM
{
    /// <summary>
    /// Allows ViewModels and Pages to be associated and new pages resolved from a ViewModel instance
    /// </summary>
    public interface IViewFactory
    {
        /// <summary>
        /// Link a ViewModel to a Page
        /// </summary>
        /// <typeparam name="TVM">ViewModel</typeparam>
        /// <typeparam name="TV">Page</typeparam>
        void Register<TVM, TV>() where TVM : BaseViewModel where TV : Page;

        /// <summary>
        /// Resolve a Page from a ViewModel
        /// </summary>
        /// <typeparam name="TVM">Type of ViewModel to resolve Page for</typeparam>
        /// <param name="setStateAction">Action used to set the ViewModel parameters</param>
        /// <returns>new Page instance</returns>
        Page Resolve<TVM>(Action<TVM> setStateAction = null) where TVM : BaseViewModel;

        /// <summary>
        /// Resolve a Page from a ViewModel
        /// </summary>
        /// <typeparam name="TVM">Type of ViewModel to resolve Page for</typeparam>
        /// <param name="viewModel">new ViewModel instance</param>
        /// <param name="setStateAction">Action used to set the ViewModel parameters</param>
        /// <returns>new Page instance</returns>
        Page Resolve<TVM>(out TVM viewModel, Action<TVM> setStateAction = null) where TVM : BaseViewModel;

        /// <summary>
        /// Resolve a Page from an existing ViewModel
        /// </summary>
        /// <typeparam name="TVM">Type of ViewModel to resolve Page for</typeparam>
        /// <param name="viewModel">existing ViewModel to get Page for</param>
        /// <returns>new Page instance</returns>
        Page Resolve<TVM>(TVM viewModel) where TVM : BaseViewModel;
    }
}