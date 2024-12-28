
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using notepad.app.Context;

namespace notepad.app;

public class DesignTimeDbContextFactory:IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<AppDbContext> dbContextOptionsBuilder = new();
        dbContextOptionsBuilder.UseSqlServer(Configuration.ConnectionString);
        return new(dbContextOptionsBuilder.Options);
    }
}