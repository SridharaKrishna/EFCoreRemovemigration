using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(ShoppingCart.DurableFunction.Startup))]

namespace ShoppingCart.DurableFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            builder.Services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(GetDBConnection()));
        }

        private static SqlConnection GetDBConnection()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            connection.AccessToken = new AzureServiceTokenProvider().GetAccessTokenAsync("https://database.windows.net/").Result;
            return connection;
        }
    }
}