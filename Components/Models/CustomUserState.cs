using BlazorApp1.Components.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorApp1.Components.Models
{
    public class CustomUserState
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        // 存储用户信息的Key
        private const string UserSessionKey = "CurrentLoginUser";

        public Login_db? CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;

        // 注入ProtectedSessionStorage
        public CustomUserState(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        // 登录时：设置用户信息 + 持久化到SessionStorage
        public async Task SetCurrentUser(Login_db user)
        {
            CurrentUser = user;
            // 持久化到浏览器（加密存储，刷新不丢）
            await _sessionStorage.SetAsync(UserSessionKey, user);
        }

        // 页面初始化时：从SessionStorage恢复用户信息
        public async Task RestoreUserState()
        {
            var result = await _sessionStorage.GetAsync<Login_db>(UserSessionKey);
            if (result.Success)
            {
                CurrentUser = result.Value;
            }
        }

        // 登出时：清空 + 删除存储
        public async Task ClearCurrentUser()
        {
            CurrentUser = null;
            await _sessionStorage.DeleteAsync(UserSessionKey);
        }
    }
}