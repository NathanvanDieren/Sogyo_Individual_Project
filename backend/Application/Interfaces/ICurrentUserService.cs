namespace Application.Interfaces;

public interface ICurrentUserService
{
    public Task SetUserByIdAsync(Guid userId);

    public bool IsAuthenticated { get; }
}