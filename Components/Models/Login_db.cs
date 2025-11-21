using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Components.Models
{
    public class Login_db
    {
        // 1. 定义自增主键
        public int Id { get; set; }

        // 2. 添加数据验证
        [Required(ErrorMessage = "用户名是必填项")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "用户名长度必须在3到50个字符之间")]
        public string Username { get; set; }

        [Required(ErrorMessage = "密码是必填项")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "密码长度必须在6到100个字符之间")]
        public string PasswordHash { get; set; } // 3. 字段名改为 PasswordHash，明确其用途
        public DateTime ? CreatedTime { get; set; }


    }
}
