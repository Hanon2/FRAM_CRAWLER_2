using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Elcometer.Demo.Xamarin.Forms.MVVM
{
    /// <summary>
    /// Base ViewModel implementation that offers helper functions used in MVVM
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy = false;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Used to signal if a asynchronous operation is taking place 
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(() => IsBusy, ref _isBusy, value); }
        }

        /// <summary>
        /// Triggered when the ViewModels view is no longer on the stack
        /// </summary>
        public virtual void ViewClosed()
        {
        }

        /// <summary>
        /// Triggered when the ViewModels view is being displayed
        /// </summary>
        public virtual void ViewEntered()
        {
        }

        /// <summary>
        /// Triggered when the ViewModels view is about to be hidden
        /// </summary>
        public virtual void ViewLeaved()
        {
        }
        
        /// <summary>
        /// Triggered when the ViewModels view is has opened
        /// </summary>
        public virtual void ViewOpened()
        {
        }

        /// <summary>
        /// Allows a property name to be extracted from an expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns>property name</returns>
        protected static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("Invalid argument", nameof(propertyExpression));

            var member = body.Member as PropertyInfo;
            if (member == null)
                throw new ArgumentException("Argument is not a property", nameof(propertyExpression));

            return member.Name;
        }

        /// <summary>
        /// Triggers the PropertyChanged event informaing observers of the change to the given property
        /// </summary>
        /// <param name="propertyName">property that has changed</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Triggers the PropertyChanged event informaing observers of the change to the given property used in the expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;

            string propertyName = GetPropertyName(propertyExpression);
            propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the backing field of the given property and updates the PropertyChanged event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="field"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        protected bool Set<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
                return false;

            field = newValue;
            OnPropertyChanged(propertyExpression);

            return true;
        }
    }
}