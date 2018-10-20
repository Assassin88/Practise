using Autofac;
using FinalTaskFacebook.Configuration;

namespace FinalTaskFacebook.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary> 
    public class ViewModelLocator
    {
        private readonly IContainer _container = ContainerConfig.Configure();

        /// <summary>
        /// Gets the StartPage view model.
        /// </summary>
        public StartPageViewModel StartPageViewModel => _container.Resolve<StartPageViewModel>();

        /// <summary>
        /// Gets the FriendsPage view model.
        /// </summary>
        public FriendsPageViewModel FriendsPageViewModel => _container.Resolve<FriendsPageViewModel>();
    }
}