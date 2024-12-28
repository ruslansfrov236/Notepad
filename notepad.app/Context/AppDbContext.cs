using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using notepad.entity;
using notepad.entity.Entities;
using notepad.entity.Entities.Common;
using notepad.entity.Entities.Identity;

namespace notepad.app.Context;

public class AppDbContext:IdentityDbContext<AppUser , AppRole , string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options){}
    
    
    public DbSet<Note> Notes { get; set; }
   public DbSet<VerificationCode> VerificationCodes { get; set; }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var datas = ChangeTracker
            .Entries<BaseEntity>();

        foreach (var data in datas)
        {
            _ = data.State switch
            {
                EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
                EntityState.Deleted=>  data.Entity.DeletedDate=DateTime.UtcNow
            };
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
  
