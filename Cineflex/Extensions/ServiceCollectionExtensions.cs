namespace Cineflex.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationSevice(this IServiceCollection services)
        {

            services.AddTransient<IUserService, UserService>()
                ;
            return services;
        }
        public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddHttpClient(typeof(Program).AssemblyQualifiedName!, client =>
            {
                client.BaseAddress = new Uri(configuration["Api:BaseUrl"]!);
                });
            string url = configuration["Api:BaseUrl"]!;
            return services;
        }
    }
}
