using Application.Interfaces;
using Domain;

namespace Application;

internal class ReviewFacade: IReviewFacade
{
    private readonly IUserRepository _userRepository;
    
    public ReviewFacade(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
}