﻿/* Bowerbird V1 

 Licensed under MIT 1.1 Public License

 Developers: 
 * Frank Radocaj : frank@radocaj.com
 * Hamish Crittenden : hamish.crittenden@gmail.com
 
 Project Manager: 
 * Ken Walker : kwalker@museum.vic.gov.au
 
 Funded by:
 * Atlas of Living Australia
 
*/

using Ninject.Modules;
using Raven.Client;
using Bowerbird.Core.CommandHandlers;
using Ninject.Extensions.Conventions;
using Microsoft.Practices.ServiceLocation;
using Bowerbird.Core.EventHandlers;
using Bowerbird.Core.DomainModels;
using SignalR;
using Bowerbird.Core.Config;

namespace Bowerbird.Web.Config
{
    public class NinjectBindingModule : NinjectModule
    {

        #region Members

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public override void Load()
        {
            // Singleton scope
            Bind<IDocumentStore>().ToProvider<NinjectRavenDocumentStoreProvider>().InSingletonScope();
            Bind<IPermissionChecker>().To<PermissionChecker>().InSingletonScope().OnActivation(x => ((PermissionChecker)x).Init());
            Bind<ISystemStateManager>().To<SystemStateManager>().InSingletonScope();

            // Request scope
            Bind<IDocumentSession>().ToProvider<NinjectRavenSessionProvider>().InRequestScope();

            // Transient scope
            Bind<IServiceLocator>().ToMethod(x => ServiceLocator.Current);
            Bind<IJsonSerializer>().To<SignalRJsonConvertAdapter>();

            Kernel.Scan(x =>
            {
                x.FromAssemblyContaining(typeof(User));
                x.FromCallingAssembly();

                x.BindingGenerators.Add(new GenericBindingGenerator(typeof(ICommandHandler<>)));
                x.BindingGenerators.Add(new GenericBindingGenerator(typeof(ICommandHandler<,>)));
                x.BindingGenerators.Add(new GenericBindingGenerator(typeof(IEventHandler<>)));
                x.BindingGenerators.Add(new DefaultBindingGenerator());

                x.Excluding<PermissionChecker>();
                x.Excluding<SystemStateManager>();
            });
        }

        #endregion

    }
}