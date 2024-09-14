using System;
using System.Threading.Tasks;

namespace Elcometer.Demo.Xamarin.Forms.MVVM
{
    /// <summary>
    /// Simple navigation service that allows ViewModels to to pushed and popped
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Pop the visible page model
        /// </summary>
        /// <returns>the ViewModel that has now been made visible after the pop</returns>
        Task<BaseViewModel> PopAsync();

        /// <summary>
        /// Push the ViewModel of type T
        /// </summary>
        /// <typeparam name="T">Type of ViewModel</typeparam>
        /// <param name="setStateAction">Action to allow ViewModel parameters to be set</param>
        /// <returns>ViewModel instance that has been pushed</returns>
        Task<T> PushAsync<T>(Action<T> setStateAction = null) where T : BaseViewModel;

        /// <summary>
        /// Push the ViewModel of type T
        /// </summary>
        /// <typeparam name="T">Type of ViewModel</typeparam>
        /// <returns>ViewModel instance that has been pushed</returns>
        Task<T> PushAsync<T>(T viewModel) where T : BaseViewModel;
    }
}