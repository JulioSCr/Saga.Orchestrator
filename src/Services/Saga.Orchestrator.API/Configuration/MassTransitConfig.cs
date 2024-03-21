﻿using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Saga.Orchestrator.Core.Messages.IntegrationContracts.Commands;

namespace Saga.Orchestrator.API.Configuration
{
    public static class MassTransitConfig
    {
        public static void AddMassTransitConfiguration(this IServiceCollection services)
        {
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(mt =>
            {
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("amqp://guest:guest@localhost:5672");

                    MessageDataDefaults.ExtraTimeToLive = TimeSpan.FromDays(1);
                    MessageDataDefaults.Threshold = 2000;
                    MessageDataDefaults.AlwaysWriteToRepository = false;
            });

                mt.AddRequestClient<ISubmitFullExport>(new Uri("exchange:submit-full-export"));
                //mt.AddRequestClient<CheckStatus>(new Uri("queue:full-export-state"));
                mt.AddRequestClient<ICheckStatus>();
            });
        }
    }
}