namespace BlazorApp1.Components.Models
{
    public class CustomUserState
    {
        // 当前登录用户信息（null表示未登录）
        public Login_db? CurrentUser { get; private set; }
        // 登录时设置用户信息
        public void SetCurrentUser(Login_db user)
        {
            CurrentUser = user;
        }

        // 登出时清空
        public void ClearCurrentUser()
        {
            CurrentUser = null;
        }

        // 判断是否登录
        public bool IsLoggedIn => CurrentUser != null;
    }
}
