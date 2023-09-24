using G1Food_Cashier.SqlDependencies;

namespace G1Food_Cashier.MiddlewareExtensions
{
    public static class ApplicationBuilderExtension
    {
        public static void UseOrderPendingDependency(this IApplicationBuilder app)
        {
            string connectionString = GetConnectionString();
            var serviceProvider = app.ApplicationServices;
            var service = serviceProvider.GetService<OrderPendingDependency>();
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
