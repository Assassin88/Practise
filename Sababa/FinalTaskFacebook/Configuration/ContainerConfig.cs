using Autofac;
using AutoMapper;
using FacebookClient.Models;
using FacebookClient.Services.Abstraction;
using FacebookClient.Services.Implementation;
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

            Mapper.Initialize(config => config.AddProfiles(typeof(Account)));

            containerBuilder.RegisterType<AuthorizationSocialNetwork>()
                .As<ISocialNetwork>().SingleInstance();

            containerBuilder.RegisterType<InitialAccount>().As<IAccount>().SingleInstance();

            containerBuilder.RegisterType<StartPageViewModel>().AsSelf();

            return containerBuilder.Build();
        }
    }
}
