using Microsoft.Extensions.DependencyInjection;

namespace CrossChained.BTP.BitIndex.Client
{
    public static class ServiceCollectionExtensions
    {
        public static void UseBitIndexClient(this IServiceCollection services)
        {
            services.AddSingleton<IBitIndexApi, impl.BitIndexApi>();
        }
    }
}
