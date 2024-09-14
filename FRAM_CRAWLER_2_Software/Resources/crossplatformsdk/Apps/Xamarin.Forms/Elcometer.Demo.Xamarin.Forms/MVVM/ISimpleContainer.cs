using System;

namespace Elcometer.Demo.Xamarin.Forms.MVVM
{
    /// <summary>
    /// Simple constructor dependancy injector
    /// </summary>
    public interface ISimpleContainer
    {
        /// <summary>
        /// Clear all the registered singletons and types
        /// </summary>
        void Clear();

        /// <summary>
        /// True if the generic type T is registered
        /// </summary>
        /// <typeparam name="T">type to check</typeparam>
        /// <returns>true if registered</returns>
        bool HasInstance(Type t);

        /// <summary>
        /// True if the generic type T is registered
        /// </summary>
        /// <typeparam name="T">type to check</typeparam>
        /// <returns>true if registered</returns>
        bool HasInstance<T>();

        /// <summary>
        /// Registers the generic implemention of type V that implements interface T
        /// </summary>
        /// <typeparam name="T">The implemention class</typeparam>
        /// <typeparam name="V">The interface for the implementation</typeparam>
        void RegisterSingleton<T, V>() where V : T;

        /// <summary>
        /// Registers the existing singleton implemention of type V that implements interface T
        /// </summary>
        /// <typeparam name="T">The implemention class</typeparam>
        /// <typeparam name="V">The interface for the implementation</typeparam>
        /// <param name="singleton">Existing singleton implementation</param>
        void RegisterSingleton<T, V>(V singleton) where V : T;

        /// <summary>
        /// Registers the generic type T
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        void RegisterType<T>();

        /// <summary>
        /// Resolves the singleton or a new instance of the given registered type
        /// </summary>
        /// <typeparam name="type">Type to return implementation of</typeparam>
        /// <returns>Singleton implementation of the instance of the registered type</returns>
        object Resolve(Type type);

        /// <summary>
        /// Resolves the singleton or a new instance of the given registered type
        /// </summary>
        /// <typeparam name="T">Type to return implementation of</typeparam>
        /// <returns>Singleton implementation of the instance of the registered type</returns>
        T Resolve<T>();

        /// <summary>
        /// Create the instance of the given type, recursively resolve its constructor arguments if required
        /// </summary>
        /// <typeparam name="of">Type to create the instance of</typeparam>
        /// <returns>new instance of the registered type</returns>
        object ResolveInstance(Type of);
    }
}