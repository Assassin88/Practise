using System;
using System.Collections.Generic;

namespace Sababa.Logic.DIContainer
{
    public class ContainerBuilder : IRegistrationBuilder
    {
        private readonly Dictionary<Type, SpecificType> _types = new Dictionary<Type, SpecificType>();
        private Type _abstractType;
        private Type _registrationType;

        public IContainer Build()
        {
            return new Container(_types);
        }

        public IRegistrationBuilder RegisterType<TImplementation>() where TImplementation : class
        {
            if (IsRegistered<TImplementation>())
            {
                throw new InvalidOperationException("It is not allowed to write a value using a registered type.");
            }

            _abstractType = typeof(TImplementation);
            return this;
        }

        public IRegistrationBuilder As<TIAbstract>() where TIAbstract : class
        {
            if (typeof(TIAbstract).IsAssignableFrom(_abstractType) && !IsRegistered<TIAbstract>())
            {
                _registrationType = typeof(TIAbstract);
                var item = new SpecificType() { ConcreteType = _abstractType };
                _types[_registrationType] = item;
                return this;
            }

            throw new InvalidOperationException("It is not allowed to write a value using a registered type.");
        }

        public IRegistrationBuilder AsSelf()
        {
            var item = new SpecificType() { ConcreteType = _abstractType };
            _types[_abstractType] = item;
            return this;
        }

        public bool UnRegisterType<TContract>()
        {
            if (IsRegistered<TContract>())
            {
                _types.Remove(typeof(TContract));
                return true;
            }

            return false;
        }

        private bool IsRegistered<TContract>()
        {
            return _types.ContainsKey(typeof(TContract));
        }

        public IRegistrationBuilder ExternallyOwned()
        {
            _types[_registrationType].IsExternallyOwned = true;
            return this;
        }

        public IRegistrationBuilder SingleInstance()
        {
            _types[_registrationType].IsSingleInstance = true;
            return this;
        }

    }
}
