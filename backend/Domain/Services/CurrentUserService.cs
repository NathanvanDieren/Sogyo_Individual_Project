using Domain.Interfaces;

namespace Domain.Services;

internal class CurrentUserService : ICurrentUserService
{
    private readonly IUserRepository _userRepository;

    public CurrentUserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User? User { get; internal set; }

    public bool IsAuthenticated => User != null;
    
    public async Task SetUserByIdAsync(Guid userId)
    {
        if (User != null) 
        {
            throw new InvalidOperationException("User is al ingesteld voor dit request.");
        }
        
        var user = await _userRepository.GetUserByIdAsync(userId);
    
        User = user ?? throw new ArgumentNullException(nameof(userId), "Gebruiker kon niet worden gevonden.");
    }
}