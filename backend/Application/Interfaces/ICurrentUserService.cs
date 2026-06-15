using Domain.Classes;

namespace Application.Interfaces;

public interface ICurrentUserService
{
    public Task SetUserByIdAsync(Guid userId);

    public bool IsAuthenticated { get; }

    public User? User { get; }
}
