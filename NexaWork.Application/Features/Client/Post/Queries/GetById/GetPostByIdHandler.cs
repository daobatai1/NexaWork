using MediatR;
using NexaWork.Application.Common.Interfaces;
using NexaWork.Application.Common.Interfaces.Repositories;
using NexaWork.Application.Common.Interfaces.Services;
using NexaWork.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace NexaWork.Application.Features.Client.Post.Queries.GetById;

public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, PostQueryDTO?>
{
    private readonly IPostRepository _postRepository;
    private readonly INexaWorkDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetPostByIdHandler(
        IPostRepository postRepository,
        INexaWorkDbContext context,
        ICurrentUserService currentUserService)
    {
        _postRepository = postRepository;
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PostQueryDTO?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null) return null;

        var userIdentityId = _currentUserService.UserId;
        var currentCustomer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdentityUserId == userIdentityId, cancellationToken);
        var currentCustomerId = currentCustomer?.CustomerId ?? Guid.Empty;

        // Check if the user is authorized to view this post
        bool isAuthorized = post.Visibility == VisibilityLevel.Public ||
                            post.CustomerId == currentCustomerId ||
                            (post.Visibility == VisibilityLevel.Connections &&
                             await _context.Connections.AnyAsync(conn =>
                                 conn.Status == ConnectionStatus.Accepted &&
                                 ((conn.CustomerId == post.CustomerId && conn.ConnectedCustomerId == currentCustomerId) ||
                                  (conn.CustomerId == currentCustomerId && conn.ConnectedCustomerId == post.CustomerId)),
                                 cancellationToken));

        if (!isAuthorized)
        {
            return null; // Return null if not authorized (hides the post)
        }

        return new PostQueryDTO(
            post.PostId,
            post.CustomerId,
            string.IsNullOrWhiteSpace(post.Customer.FirstName) && string.IsNullOrWhiteSpace(post.Customer.LastName)
                    ? "Anonymous User" // If both are null
                    : (post.Customer.FirstName + " " + post.Customer.LastName).Trim(), // Trim to remove any extra space if one of them is null
                 
            post.Customer.ProfilePictureUrl,
            post.Content,
            post.MediaUrl,
            post.LikesCount,
            post.CommentsCount,
            post.SharesCount,
            post.Visibility,
            post.CreatedAt,
            post.UpdatedAt
        );
    }
}

