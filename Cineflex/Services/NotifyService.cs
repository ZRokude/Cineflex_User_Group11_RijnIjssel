 namespace Cineflex.Services
{
    public class NotifyService()
    {
        public event Action<HashSet<Type>>? ForceRefresh;
        public event Action<Type>? RefreshCompleted;
        //public event Action<NotificationModel>? DisplayNotification;
        public event Action? ForceLogout;

        public void InvokeRefreshOnComponents(HashSet<Type> componentsToRefresh) => ForceRefresh?.Invoke(componentsToRefresh);
        public void InvokeRefreshCompleted(Type refreshedComponent) => RefreshCompleted?.Invoke(refreshedComponent);
        //public void InvokeDisplayNotification(NotificationModel model)
        //{
        //    model.Text = localizer[model.Text];

        //    DisplayNotification?.Invoke(model);
        //}
        public void InvokeForceLogout() => ForceLogout?.Invoke();
    }
}
