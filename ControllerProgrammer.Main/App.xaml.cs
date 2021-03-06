﻿using ControllerProgrammer.Main.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using ControllerProgrammer.ProgramForm;
using ControllerProgrammer.Data.Model;
using Prism.DryIoc;
using DryIoc;
using ControllerProgrammer.Common.Interfaces;
using ControllerProgrammer.Common.Services;
using DevExpress.Xpf.Core;

namespace ControllerProgrammer.Main {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App {
        protected override void OnStartup(StartupEventArgs e) {
            //ApplicationThemeHelper.ApplicationThemeName = Theme.VS2017DarkName;
            base.OnStartup(e);
        }

        protected override Window CreateShell() {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) {
            var container = containerRegistry.GetContainer();
            container.With(rules => rules.WithoutImplicitCheckForReuseMatchingScope());

            //container.Register<ProgrammerContext>(setup:A)
            container.Register<ProgrammerContext>(setup: Setup.With(allowDisposableTransient: true));
            containerRegistry.RegisterSingleton<IControllerManager, ControllerManager>();
            containerRegistry.Register<IControllerDataManagment, ControllerDataManagment>();
            //containerRegistry.Register<IControllerManager, ControllerManager>();
            //containerRegistry.RegisterInstance<IControllerManager>(new ControllerManager());
           
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) {
            moduleCatalog.AddModule<ProgramFormModule>();
        }
    }
}
