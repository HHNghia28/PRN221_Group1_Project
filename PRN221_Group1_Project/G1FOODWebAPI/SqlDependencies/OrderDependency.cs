using G1FOODLibrary.Entities;
using G1FOODWebAPI.Hubs;
using TableDependency.SqlClient;

namespace G1FOODWebAPI.SqlDependencies
{
    public class OrderDependency
    {
        private readonly OrderHub orderHub;
        SqlTableDependency<Order> _oderDependency;

        public OrderDependency(OrderHub orderHub)
        {
            this.orderHub = orderHub;
        }

        public void Subcribe()
        {
            string connectionString = GetConnectionString();
            _oderDependency = new SqlTableDependency<Order>(connectionString);
            _oderDependency.OnChanged += OderDependency_OnChanged;
            _oderDependency.OnError += OderDependency_OnError;
            _oderDependency.Start();
        }

        private void OderDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Order)} SqlTableDependency error: {e.Error.Message}");
        }

        private void OderDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Order> e)
        {
            if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Insert)
            {
                orderHub.SendOrderPendingAsync();
            }
            if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Update)
            {
                orderHub.SendOrderDeliveringingAsync();
                orderHub.SendOrderCookingAsync();
            }
        }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var strConn = config["ConnectionString:G1FoodDB"];
            return strConn;
        }
    }
}
