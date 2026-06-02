using Domain.Interfaces;

namespace Domain;

internal class ReviewFacade: IReviewFacade
{
    private readonly IUserRepository _userRepository;
    
    public ReviewFacade(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
}