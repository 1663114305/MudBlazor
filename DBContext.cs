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

    public DbSet<Photo> Photos { get; set; }

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

        // Login_db 配置
        modelBuilder.Entity<Login_db>()
            .HasIndex(l => l.Username)
            .IsUnique();

        // Photo 配置
        modelBuilder.Entity<Photo>(entity =>
        {
            // 设置表名
            entity.ToTable("Photos");

            // 创建索引
            entity.HasIndex(e => e.Username); // 按用户查询索引
            entity.HasIndex(e => e.UploadTime); // 按时间查询索引
            entity.HasIndex(e => e.IsPublic); // 按公开状态查询索引

            // 字段长度配置
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.OriginalFileName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.FilePath)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.FileType)
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .HasMaxLength(500);
        });
    }
}