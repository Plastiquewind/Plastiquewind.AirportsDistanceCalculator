using System;
using Plastiquewind.AirportsDistanceCalculator.Application.Airports;
using Plastiquewind.AirportsDistanceCalculator.Application.Contracts;
using Plastiquewind.AirportsDistanceCalculator.Application.Contracts.Converters;
using Plastiquewind.AirportsDistanceCalculator.Application.Converters;
using Plastiquewind.AirportsDistanceCalculator.Shared;

using Autofac;
using Flurl.Http.Configuration;

namespace Plastiquewind.AirportsDistanceCalculator.Application
{
    public sealed class ApplicationModule : Module
    {
        private readonly IApplicationModuleSettings settings;

        public ApplicationModule(IApplicationModuleSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        protected sealed override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(settings).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PerBaseUrlFlurlClientFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MetersToMilesConverterApplicationService>()
                .Keyed<IMetersConverterApplicationService>(DistanceUnit.Mile)
                .InstancePerLifetimeScope();
            builder.RegisterType<AirportsApplicationService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
