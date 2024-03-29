﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Athena.RabbitMQHelper;

public static class ServiceCollectionExtensions
{
    public static void AddRabbit(this IServiceCollection services, IConfigurationSection section)
    {
        var test = section.GetValue<string>("Rabbit");
        services.AddSingleton<IBus>(s => RabbitHutch.CreateBus(
            section.GetValue<string>("Connection")));
    }
}