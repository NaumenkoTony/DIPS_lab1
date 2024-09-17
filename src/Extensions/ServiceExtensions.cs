namespace DIPS_lab01.ServiceExtentions;

using DIPS_lab01.Data;
using DIPS_lab01.Data.Repositories;
using DIPS_lab01.Models;

public static class ServiceExtensions
{
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Person>, Repository<Person>>();
    }
}