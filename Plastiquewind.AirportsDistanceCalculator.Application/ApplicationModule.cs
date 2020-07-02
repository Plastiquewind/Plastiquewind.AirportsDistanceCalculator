using System;
using System.Runtime.CompilerServices;
using Plastiquewind.AirportsDistanceCalculator.Application.Airports;
using Plastiquewind.AirportsDistanceCalculator.Application.Contracts;
using Plastiquewind.AirportsDistanceCalculator.Application.Contracts.Converters;
using Plastiquewind.AirportsDistanceCalculator.Application.Converters;
using Plastiquewind.AirportsDistanceCalculator.Shared;

using Autofac;
using Flurl.Http.Configuration;

[assembly: InternalsVisibleTo("Plastiquewind.AirportsDistanceCalculator.Application.UnitTests")]

namespace Plastiquewind.AirportsDistanceCalculator.Application
{
    public sealed class ApplicationModule : Module
    {
        #region Fields

        private readonly IApplicationModuleSettings settings;

        #endregion

        #region Constructors

        public ApplicationModule(IApplicationModuleSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        #endregion

        #region Public methods

        protected sealed override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(settings).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PerBaseUrlFlurlClientFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MetersToMilesConverterApplicationService>()
                .Keyed<IMetersConverterApplicationService>(DistanceUnit.Mile)
                .InstancePerLifetimeScope();
            builder.RegisterType<AirportsApplicationService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        #endregion
    }
}
