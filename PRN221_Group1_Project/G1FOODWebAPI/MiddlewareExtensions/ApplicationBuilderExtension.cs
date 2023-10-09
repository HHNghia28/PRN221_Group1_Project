using G1FOODWebAPI.SqlDependencies;

namespace G1FOODWebAPI.MiddlewareExtensions
{
    public static class ApplicationBuilderExtension
    {
        public static void UseOrderPendingDependency(this IApplicationBuilder app)
        {
            string connectionString = GetConnectionString();
            var serviceProvider = app.ApplicationServices;
            var service = serviceProvider.GetService<OrderDependency>();
            service.Subcribe();
        }

        private static string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var strConn = config["ConnectionString:G1FoodDB"];
            return strConn;
        }
    }
}
