using BlazorApp1.Components.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1;

public class DBContext : DbContext
{
    // 2. 构造函数，调用基类构造函数
    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    // 3. 定义 DbSet，表示数据库中的表
    public DbSet<Login_db> Login_db { get; set; }

    // 4. (推荐) 重写 OnConfiguring 方法作为后备配置
    //    当在 Program.cs 中配置后，这里的配置会被覆盖
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // 仅当没有外部配置时才执行这里的代码
        if (!optionsBuilder.IsConfigured)
        {
            // 示例：配置为使用 SQLite
            optionsBuilder.UseSqlite("Data Source=BlazorApp1.db");
        }
    }

    // 5. (可选) 重写 OnModelCreating 方法来配置实体关系
    //    如果你的实体关系比较复杂，需要在这里配置
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Login_db>()
            .HasIndex(l => l.Username) 
            .IsUnique(); 
    }
}