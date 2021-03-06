﻿/* Bowerbird V1 - Licensed under MIT 1.1 Public License

 Developers: 
 * Frank Radocaj : frank@radocaj.com
 * Hamish Crittenden : hamish.crittenden@gmail.com
 
 Project Manager: 
 * Ken Walker : kwalker@museum.vic.gov.au
 
 Funded by:
 * Atlas of Living Australia
 
*/

using System;
using Bowerbird.Core.Config;
using Microsoft.AspNet.SignalR;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using System.Web;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Bowerbird.Web.Infrastructure.NinjectBootstrapper), "PreStart", Order = 1)]
[assembly: WebActivator.PreApplicationStartMethod(typeof(Bowerbird.Web.Infrastructure.NinjectBootstrapper), "PostStart", Order = 1)]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Bowerbird.Web.Infrastructure.NinjectBootstrapper), "Stop")]

namespace Bowerbird.Web.Infrastructure
{
    public static class NinjectBootstrapper
    {
        private static readonly Bootstrapper _ninjectBootstrapper = new Bootstrapper();

        public static void PreStart()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            _ninjectBootstrapper.Initialize(CreateKernel);
        }

        public static void PostStart()
        {
            GlobalHost.DependencyResolver = new SignalrNinjectDependencyResolver(_ninjectBootstrapper.Kernel);

            _ninjectBootstrapper.Kernel.Get<ISystemStateManager>().SetupSystem();
        }

        public static void Stop()
        {
            _ninjectBootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            
            return kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(new NinjectBindingModule());
        }
    }
}