using MediatR;
using NexaWork.Application.Common.Interfaces;
using NexaWork.Application.Common.Interfaces.Repositories;
using NexaWork.Application.Common.Interfaces.Services;
using NexaWork.Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace NexaWork.Application.Features.Client.Post.Queries.GetAll;

public class GetAllPostsHandler : IRequestHandler<GetAllPostsQuery, List<PostQueryDTO>>
{
    //private readonly IPostRepository _postRepository;
    private readonly INexaWorkDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetAllPostsHandler(
        //IPostRepository postRepository,
        INexaWorkDbContext context,
        ICurrentUserService currentUserService
    )
    {
        //_postRepository = postRepository;
        _context = context;
        _currentUserService = currentUserService;
    }
    public async Task<List<PostQueryDTO>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        var userIdentityId = _currentUserService.UserId;
        var currentCustomer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdentityUserId == userIdentityId, cancellationToken);
        var currentCustomerId = currentCustomer?.CustomerId ?? Guid.Empty;

        //var customerAvatar = currentCustomer?.ProfilePictureUrl ?? string.Empty;

        return await _context.Posts
            .AsNoTracking()
            .Where(post =>
                post.Visibility == VisibilityLevel.Public ||
                post.CustomerId == currentCustomerId ||
                (post.Visibility == VisibilityLevel.Connections &&
                 _context.Connections.Any(conn =>
                     conn.Status == ConnectionStatus.Accepted &&
                     ((conn.CustomerId == post.CustomerId && conn.ConnectedCustomerId == currentCustomerId) ||
                      (conn.CustomerId == currentCustomerId && conn.ConnectedCustomerId == post.CustomerId))
                 ))
            )
            //.Include(async post  => post.CustomerId == await _customerRepository.GetCustomerByIdAsync(post.CustomerId, cancellationToken))
            .OrderByDescending(p => p.CreatedAt)
            .Select(post => new PostQueryDTO
            (
                post.PostId,
                post.CustomerId,
                string.IsNullOrWhiteSpace(post.Customer.FirstName) && string.IsNullOrWhiteSpace(post.Customer.LastName)
                    ? "Anonymous User" // If both are null
                    : (post.Customer.FirstName + " " + post.Customer.LastName).Trim(), // Trim to remove any extra space if one of them is null

                //post.Customer.ProfilePictureUrl,
                string.IsNullOrEmpty(post.Customer.ProfilePictureUrl) ? null : post.Customer.ProfilePictureUrl,
                //customerAvatar,
                post.Content,
                post.MediaUrl,
                post.LikesCount,
                post.CommentsCount,
                post.SharesCount,
                post.Visibility,
                post.CreatedAt,
                post.UpdatedAt
            ))
            .ToListAsync(cancellationToken);
    }
}

// 
