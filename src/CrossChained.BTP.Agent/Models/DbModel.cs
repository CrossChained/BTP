using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace Models
{
    internal class DbModel : DbContext
    {
        public DbModel(DbContextOptions options)
    : base(options)
        {
            if (!is_inited_)
            {
                lock (typeof(DbModel))
                {
                    if (!is_inited_)
                    {
                        var config = this.GetService<Microsoft.Extensions.Configuration.IConfiguration>();
                        if (config != null)
                        {
                            var section = config.GetSection("APICFG_ALLOWMIGRATION");
                            if (section.Value == "true")
                            {
                                InitDatabase(this);
                            }
                        }

                        is_inited_ = true;
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        private static bool is_inited_ = false;
        private static void InitDatabase(DbModel model)
        {
            model.Database.Migrate();
            if (model.AllMigrationsApplied())
            {
                model.EnsureSeedData();
            }
        }

        private void EnsureSeedData()
        {

        }

        private bool AllMigrationsApplied()
        {
            var applied = this.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = this.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

    }
}