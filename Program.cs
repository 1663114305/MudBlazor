// 导入 Blazor 应用的核心组件命名空间（包含共享组件、页面组件等基础结构）
using BlazorApp1;
using BlazorApp1.Components;
using BlazorApp1.Components.Models;
// 导入 Pages 文件夹下的页面组件命名空间（如 Login.razor、Home.razor 等页面）
using BlazorApp1.Components.Pages;
// 导入数据相关命名空间（可能包含数据模型、数据处理工具类等）
using BlazorApp1.Data;
// 导入自定义模型命名空间（如 Student、User 等实体类）
using BlazorApp1.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
// 导入 MudBlazor 组件库的服务命名空间（用于使用 MudBlazor 的 UI 组件和功能）
using MudBlazor.Services;


// 创建 Web 应用程序构建器，用于配置服务、中间件等应用核心设置
var builder = WebApplication.CreateBuilder(args);

// 向依赖注入容器添加 Razor 组件服务
// AddInteractiveServerComponents() 启用 Blazor Server 交互式渲染模式（组件逻辑在服务器执行）
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 向依赖注入容器添加 MudBlazor 组件库的服务
// 启用 MudBlazor 的主题、组件交互等核心功能（如按钮、表单、对话框等组件）
builder.Services.AddMudServices();

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 524288000; // 500MB
});
// 注册自定义仓储服务（依赖注入：面向接口编程）
// AddScoped：作用域生命周期（每次 HTTP 请求创建一个新实例，请求结束释放）
// IStudentRepository：仓储接口（定义数据操作规范）
// StudentRepository：接口的实现类（实际处理学生数据的增删改查）
builder.Services.AddScoped<IStudentRepository, StudentRepository>();


builder.Services.AddDbContext<DBContext>(options =>
                options.UseSqlite("Data Source=MySqlite.db"));
builder.Services.AddScoped<ProtectedSessionStorage>();
// 注册用户状态服务（Scoped，每个会话独立）
builder.Services.AddScoped<CustomUserState>();

// 构建 Web 应用程序实例（完成服务配置，生成可运行的应用）
var app = builder.Build();

// --- 在这里添加数据库初始化代码 ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // 获取 DBContext 实例
        var context = services.GetRequiredService<DBContext>();

        // 检查数据库是否存在，如果不存在则创建数据库和表
        // 如果数据库已存在，此方法不做任何事情
        context.Database.EnsureCreated();


        var defaultUsers = new List<Login_db>
        {
            new Login_db { Username = "admin", PasswordHash = "123456",email = "11111",CreatedTime = DateTime.Now ,LastLoginTime = DateTime.Now,quanxian = 4 },
            new Login_db { Username = "root", PasswordHash = "123456" ,email = "11111",CreatedTime = DateTime.Now ,LastLoginTime = DateTime.Now ,quanxian = 4 } ,
            new Login_db { Username = "lsw", PasswordHash = "123456",email = "11111",CreatedTime = DateTime.Now ,LastLoginTime = DateTime.Now ,quanxian = 4  } ,
            new Login_db { Username = "cyj", PasswordHash = "123456" ,email = "11111",CreatedTime = DateTime.Now ,LastLoginTime = DateTime.Now ,quanxian = 4 } ,
            new Login_db { Username = "lr", PasswordHash = "123456",email = "11111" ,CreatedTime = DateTime.Now ,LastLoginTime = DateTime.Now,quanxian = 4  } ,

        };


        foreach (var user in defaultUsers)
        {
            // 检查账号是否已存在
            var existingUser = await context.Login_db
                                           .Where(u => u.Username == user.Username)
                                           .FirstOrDefaultAsync();

            if (existingUser == null)
            {
                // 如果不存在，则创建新账号
                var newUser = new Login_db
                {
                    Username = user.Username,
                    CreatedTime = DateTime.Now,
                    PasswordHash  = user.PasswordHash,
                    email = user.email,
                    LastLoginTime = DateTime.Now,
                    quanxian = user.quanxian,
                    
                };
                // 对密码进行哈希处理

                context.Login_db.Add(newUser);
                Console.WriteLine($"默认账号 '{user.Username}' 已创建。");
            }
            else
            {
                Console.WriteLine($"默认账号 '{user.Username}' 已存在，跳过创建。");
            }
        }

        // 保存更改到数据库
        await context.SaveChangesAsync();
    
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "数据库创建失败");
    }
}

// 配置 HTTP 请求处理管道（中间件顺序决定请求处理流程）
// 判断当前环境是否为开发环境（通过环境变量 ASPNETCORE_ENVIRONMENT 配置）
if (!app.Environment.IsDevelopment())
{
    // 非开发环境：启用异常处理中间件，指定错误页面路由为 "/Error"
    // createScopeForErrors: true：创建独立的依赖注入作用域处理异常，避免影响主作用域
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // 非开发环境：启用 HTTP 严格传输安全协议（HSTS）
    // 强制浏览器后续仅通过 HTTPS 访问应用，提升安全性
    app.UseHsts();
}

// 启用 HTTPS 重定向中间件：将 HTTP 请求自动重定向到 HTTPS
app.UseHttpsRedirection();
// 启用静态文件中间件：允许访问 wwwroot 文件夹下的静态资源（如 CSS、JS、图片等）
app.UseStaticFiles();
// 启用防跨站请求伪造（CSRF）中间件：保护表单提交等 POST 请求的安全性
app.UseAntiforgery();

// 映射 Razor 组件到应用，设置根组件为 App 组件（App.razor）
// AddInteractiveServerRenderMode()：指定默认使用 Blazor Server 交互式渲染模式
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 启动 Web 应用程序，开始监听 HTTP/HTTPS 请求
app.Run();