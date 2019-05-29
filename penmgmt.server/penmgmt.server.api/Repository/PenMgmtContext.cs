using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PenMgmt.Common.Domain;
using PenMgmt.Common.Helper;
using PenMgmt.Server.Api.Configuration;

namespace PenMgmt.Server.Persistence.Repository
{
    public class PenMgmtContext : DbContext
    {
        public AppSettingsDataStore AppSettingsDataStore { get; private set; }
        private IAppLogger _appLogger;

        public PenMgmtContext (
            IAppLogger appLogger,
            IOptions<AppSettingsDataStore> appSettingsDataStore)
        {
            _appLogger = appLogger;
            AppSettingsDataStore = appSettingsDataStore.Value;
        }

        public PenMgmtContext (
            IAppLogger appLogger,
            AppSettingsDataStore appSettings)
        {
            _appLogger = appLogger;
            AppSettingsDataStore = appSettings;
        }

        public DbSet<PartMaster> PartMasters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _appLogger.LogError(message: $"DataStore Type: '{AppSettingsDataStore.Type}', ConnectionString: '{AppSettingsDataStore.ConnectionString}'");
            switch(AppSettingsDataStore.Type)
            {
                case "InMemoryDatabase":
                    optionsBuilder.UseInMemoryDatabase(AppSettingsDataStore.Name, null);
                    break;
                case "Sqlite":    
                    optionsBuilder.UseSqlite(AppSettingsDataStore.ConnectionString);
                    break;
                default:
                    _appLogger.LogError(message: $"PenMgmtContext::OnConfiguring() >> Invalid DataStore Type '{AppSettingsDataStore.Type}' configured. NOT SUPPORTED.");
                    // TODO: Throw error and exit
                    break;
            }
        }
    }
}