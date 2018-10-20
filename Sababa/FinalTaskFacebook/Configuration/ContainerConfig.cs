using Autofac;
using AutoMapper;
using FacebookClient.Models;
using FacebookClient.Services.Abstraction;
using FacebookClient.Services.Implementation;
using FinalTaskFacebook.Services.Implementation;
using FinalTaskFacebook.ViewModels;
using FinalTaskFacebook.Views;
using GalaSoft.MvvmLight.Views;

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

            var navigationService = CreateNavigationService();
            containerBuilder.RegisterInstance(navigationService).As<INavigationService>().SingleInstance();
            containerBuilder.RegisterType<StartPageViewModel>().AsSelf();
            containerBuilder.RegisterType<FriendsPageViewModel>().AsSelf();

            Mapper.Initialize(config => config.AddProfiles(typeof(Account)));

            containerBuilder.RegisterType<AuthorizationSocialNetwork>()
                .As<ISocialNetwork>().SingleInstance();

            containerBuilder.RegisterType<InitialAccount>().As<IAccount>().SingleInstance();

            return containerBuilder.Build();
        }

        private static INavigationService CreateNavigationService()
        {
            var service = new NavigationService();
            service.Configure("FriendsPage", typeof(FriendsPage));
            return service;
        }
    }
}