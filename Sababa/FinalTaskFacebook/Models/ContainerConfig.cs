using Autofac;
using FinalTaskFacebook.ViewModels;

namespace FinalTaskFacebook.Models
{
    public static class ContainerConfig
    {
        /// <summary>
        /// Create a new container with the component registrations that have been made.
        /// </summary>
        /// <returns>IContainer instance.</returns>
        public static IContainer Configure()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<StartPageViewModel>().AsSelf();

            containerBuilder.RegisterType<FacebookSocialNetwork>()
                .As<IFacebookSocialNetwork>().SingleInstance();

            containerBuilder.RegisterType<FacebookAgent>()
                .As<IFacebookAgent>().SingleInstance();

            return containerBuilder.Build();
        }
    }
}
