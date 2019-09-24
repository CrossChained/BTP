using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace CrossChained.BTP.NBitcoinSV.Client
{
    public static class ServiceCollectionExtensions
    {
        public static void UseBitcoinSVClient(this IServiceCollection services)
        {
            services.AddSingleton<IBitcoinSVApi, impl.BitcoinSVApi>();
        }
    }
}
