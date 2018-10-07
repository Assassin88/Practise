using System;

namespace Sababa.Logic.DIContainer
{
    public interface IContainer : IDisposable
    {
        /// <summary>
        /// Returns an instance of an object registered with this type.
        /// </summary>
        /// <typeparam name="TImplementation"></typeparam>
        /// <returns></returns>
        TImplementation Resolve<TImplementation>();
    }
}