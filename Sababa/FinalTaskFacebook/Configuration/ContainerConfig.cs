using Autofac;
using AutoMapper;
using FinalTaskFacebook.Services.Abstraction;
using FinalTaskFacebook.Services.Implementation;
using FinalTaskFacebook.ViewModels;

namespace FinalTaskFacebook.Configuration
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

            Mapper.Initialize(config => config.AddProfiles(typeof(App)));

            containerBuilder.RegisterType<StartPageViewModel>().AsSelf();

            containerBuilder.RegisterType<FacebookSocialNetwork>()
                .As<ISocialNetwork>().SingleInstance();

            return containerBuilder.Build();
        }
    }
}
