using System.Reflection;

namespace Elcometer.Demo.Xamarin.Forms.MVVM
{
    public class SimpleContainer : ISimpleContainer
    {
        private readonly HashSet<Type> _registeredTypes = new HashSet<Type>();
        private readonly Dictionary<Type, object> _singletons = new Dictionary<Type, object>();
        private readonly Dictionary<Type, Type> _singletonTypes = new Dictionary<Type, Type>();

        public static SimpleContainer Instance;

        public void Clear()
        {
            _registeredTypes.Clear();
            _singletonTypes.Clear();
            _singletons.Clear();
        }

        public bool HasInstance<T>()
        {
            return HasInstance(typeof(T));
        }

        public bool HasInstance(Type t)
        {
            return _singletonTypes.ContainsKey(t);
        }

        public void RegisterSingleton<T, V>(V singleton) where V : T
        {
            _singletonTypes[typeof(T)] = typeof(V);
            _singletons[typeof(T)] = singleton;
        }

        public void RegisterSingleton<T, V>() where V : T
        {
            _singletonTypes[typeof(T)] = typeof(V);
        }

        public void RegisterType<T>()
        {
            _registeredTypes.Add(typeof(T));
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            object service;
            if (_singletons.TryGetValue(type, out service))
            {
                return service;
            }

            Type serviceType;
            if (_singletonTypes.TryGetValue(type, out serviceType))
            {
                var instance = ResolveInstance(serviceType);
                _singletons[type] = instance;
                return instance;
            }

            if (_registeredTypes.Contains(type))
            {
                return ResolveInstance(type);
            }

            throw new Exception($"Service not found for type '{type}'");
        }

        public object ResolveInstance(Type of)
        {
            var info = of.GetTypeInfo();

            foreach (var constructor in info.DeclaredConstructors.ToList())
            {
                if (constructor.GetParameters().Count() == 0)
                    continue;

                var canFillAllParameters = constructor.GetParameters().All(p => HasInstance(p.ParameterType));

                if (!canFillAllParameters)
                    continue;

                var parameters = (from p in constructor.GetParameters()
                                  let r = Resolve(p.ParameterType)
                                  select r).ToArray();

                return constructor.Invoke(parameters);
            }

            return Activator.CreateInstance(of);
        }
    }
}