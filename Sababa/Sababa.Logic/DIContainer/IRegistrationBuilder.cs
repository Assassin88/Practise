namespace Sababa.Logic.DIContainer
{
    public interface IRegistrationBuilder
    {
        /// <summary>
        /// Registered type to type collection.
        /// </summary>
        /// <typeparam name="TImplementation"></typeparam>
        /// <returns></returns>
        IRegistrationBuilder RegisterType<TImplementation>() where TImplementation : class;

        /// <summary>
        /// Registers a value of a registered type to a type collection.
        /// </summary>
        /// <typeparam name="TIAbstract"></typeparam>
        /// <returns></returns>
        IRegistrationBuilder As<TIAbstract>() where TIAbstract : class;

        /// <summary>
        /// Registers the value of the registered type as itself in the type collection.
        /// </summary>
        /// <returns></returns>
        IRegistrationBuilder AsSelf();

        /// <summary>
        /// Returns a collection of registered types.
        /// </summary>
        /// <returns></returns>
        IContainer Build();

        /// <summary>
        /// Mark an object for deletion.
        /// </summary>
        /// <returns></returns>
        IRegistrationBuilder ExternallyOwned();

        /// <summary>
        /// Marks an object as a singleton
        /// </summary>
        /// <returns></returns>
        IRegistrationBuilder SingleInstance();

        /// <summary>
        /// Deletes an object by the specified type.
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <returns></returns>
        bool UnRegisterType<TContract>();
    }
}
