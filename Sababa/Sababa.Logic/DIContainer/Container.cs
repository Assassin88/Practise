using System;
using System.Collections.Generic;

namespace Sababa.Logic.DIContainer
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, SpecificType> _types;
        private readonly List<IDisposable> _disposableObjects = new List<IDisposable>();
        private readonly Dictionary<Type, object> _singleObjects = new Dictionary<Type, object>();

        internal Container(Dictionary<Type, SpecificType> types)
        {
            _types = types;
        }

        public TImplementation Resolve<TImplementation>()
        {
            return (TImplementation)Resolve(typeof(TImplementation));
        }

        private bool IsRegisteredType(Type type)
        {
            return _types.ContainsKey(type);
        }

        private object Resolve(Type type)
        {
            if (!IsRegisteredType(type))
                throw new ArgumentException("This type is not registered.");

            var currentType = _types[type];
            if (currentType.IsSingleInstance == true && _singleObjects.ContainsKey(type))
            {
                return _singleObjects[type];
            }

            object result;
            var constructor = currentType.ConcreteType.GetConstructors()[0];
            if (constructor.GetParameters().Length == 0)
            {
                result = Activator.CreateInstance(currentType.ConcreteType);
                if (currentType.IsSingleInstance)
                {
                    _singleObjects.Add(type, result);
                }
                if (!currentType.IsExternallyOwned && result is IDisposable disposable)
                {
                    _disposableObjects.Add(disposable);
                }

                return result;
            }
            else
            {
                var parameters = new List<object>();
                foreach (var item in constructor.GetParameters())
                {
                    parameters.Add(Resolve(item.ParameterType));
                }

                result = Activator.CreateInstance(currentType.ConcreteType, parameters.ToArray());
                if (currentType.IsSingleInstance)
                {
                    _singleObjects.Add(type, result);
                }
                if (!currentType.IsExternallyOwned && result is IDisposable disposable)
                {
                    _disposableObjects.Add(disposable);
                }

                return result;
            }
        }

        #region IDisposable 

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Clean();
                }

                disposedValue = true;
            }
        }

        private void Clean()
        {
            foreach (var item in _disposableObjects)
            {
                item.Dispose();
            }

            foreach (var singleObject in _singleObjects.Values)
            {
                if (singleObject is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        ~Container()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
