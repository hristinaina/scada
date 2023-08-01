using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace scada.Database
{
    public class Startup
    {
        // registering mysql connection for dependency injection
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Other service registrations...

            string connectionString = Configuration.GetConnectionString("MySqlConnection");
            services.AddTransient<MySqlConnection>(_ => new MySqlConnection(connectionString));
        }

    }
}
