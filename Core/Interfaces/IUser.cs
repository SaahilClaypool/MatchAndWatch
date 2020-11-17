namespace Core.Interfaces {
    public interface IUser {
        string Id { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
    }

    public interface INotification {
        string Message { get; }
    }
}
