namespace ReaderForArcGISNews
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;

    using Autofac;

    using Caliburn.Micro;

    using Microsoft.Phone.Controls;

    using ReaderForArcGISNews.Framework;

    using IoC = ReaderForArcGISNews.Framework.IoC;

    public class AppBootstrapper : PhoneBootstrapper
    {
        private IContainer container;

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var config = new TypeMappingConfiguration
                             {
                                 DefaultSubNamespaceForViewModels = "ViewModels",
                                 DefaultSubNamespaceForViews = "Views"
                             };

            ViewLocator.ConfigureTypeMappings(config);
            ViewModelLocator.ConfigureTypeMappings(config);

            base.OnStartup(sender, e);
        }

        protected override void Configure()
        {
            // configure container
            var builder = new ContainerBuilder();

            // register phone services
            var caliburnAssembly = typeof(IStorageMechanism).Assembly;

            // register IStorageMechanism implementors
            builder.RegisterAssemblyTypes(caliburnAssembly)
                   .Where(
                       type => typeof(IStorageMechanism).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                   .As<IStorageMechanism>()
                   .InstancePerLifetimeScope();

            // register IStorageHandler implementors
            builder.RegisterAssemblyTypes(caliburnAssembly)
                   .Where(
                       type => typeof(IStorageHandler).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                   .As<IStorageHandler>()
                   .InstancePerLifetimeScope();

            // register services
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                   .Where(type => type.Name.EndsWith("Service") && type.IsClass && !type.IsAbstract)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            // register view models
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                   .Where(type => type.Name.EndsWith("ViewModel"))
                   .AsSelf()
                   .InstancePerLifetimeScope();

            // register views
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                   .Where(type => type.Name.EndsWith("View"))
                   .AsSelf()
                   .InstancePerLifetimeScope();

            builder.RegisterInstance<IPhoneContainer>(new AutofacPhoneContainer()).SingleInstance();

            // register the singletons
            var frameAdapter = new FrameAdapter(this.RootFrame);
            var phoneServices = new PhoneApplicationServiceAdapter(this.RootFrame);

            builder.RegisterInstance(frameAdapter).As<INavigationService>();
            builder.RegisterInstance(phoneServices).As<IPhoneService>();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();
            builder.RegisterType<StorageCoordinator>().AsSelf().SingleInstance();
            builder.RegisterType<TaskController>().AsSelf().SingleInstance();

            // build the container
            this.container = builder.Build();

            // start services
            this.container.Resolve<StorageCoordinator>().Start();
            this.container.Resolve<TaskController>().Start();

            // Initialize IoC-Container utility
            IoC.Initialize(this.container);

            AddCustomConventions();
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this.container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
        }

        protected override PhoneApplicationFrame CreatePhoneApplicationFrame()
        {
            return new TransitionFrame();
        }

        protected override object GetInstance(Type service, string key)
        {
            object instance;
            if (string.IsNullOrEmpty(key))
            {
                if (this.container.TryResolve(service, out instance))
                {
                    return instance;
                }
            }
            else
            {
                if (this.container.TryResolveNamed(key, service, out instance))
                {
                    return instance;
                }
            }

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", key ?? service.Name));
        }

        protected override void BuildUp(object instance)
        {
            this.container.InjectUnsetProperties(instance);
        }

        protected override void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {

#if DEBUG
            MessageBox.Show(e.ExceptionObject.ToString());
            Debug.WriteLine("Error : {0}", e.ExceptionObject);
#endif

            if (e.ExceptionObject is QuitException)
            {
                return;
            }

            if (e.ExceptionObject is NetworkConnectionException)
            {
                MessageBox.Show("Check your connection and try again.", "No internet access", MessageBoxButton.OK);
                return;
            }

            MessageBox.Show(
                "Please try later again. If the problem continues, take contact to publisher.",
                "Hups, there was an error",
                MessageBoxButton.OK);
        }

        private static void AddCustomConventions()
        {
        }
    }
}