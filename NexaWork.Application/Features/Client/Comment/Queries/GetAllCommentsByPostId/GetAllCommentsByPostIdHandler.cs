using MediatR;
using NexaWork.Application.Common.Interfaces.Repositories;
using NexaWork.Domain.Entities;

namespace NexaWork.Application.Features.Client.Comment.Queries.GetAllCommentsByPostId;

public class GetAllCommentsByPostIdHandler : IRequestHandler<GetAllCommentsByPostIdQuery, List<CommentQueryDTO>>
{
    private readonly ICommentRepository _commentRepository;


    public GetAllCommentsByPostIdHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<List<CommentQueryDTO>> Handle(GetAllCommentsByPostIdQuery request,
        CancellationToken cancellationToken)
    {
        var comments = await _commentRepository.GetAllCommentByPostIdAsync(request.PostId, cancellationToken);

        return comments.Select(comment => new CommentQueryDTO(
             string.IsNullOrWhiteSpace(comment.Customer.FirstName) && string.IsNullOrWhiteSpace(comment.Customer.LastName)
                    ? "Anonymous User" // If both are null
                    : (comment.Customer.FirstName + " " + comment.Customer.LastName).Trim(),

            //string.IsNullOrEmpty(post.Customer.ProfilePictureUrl) ? null : post.Customer.ProfilePictureUrl,

            comment.CommentId,
            comment.PostId,
            comment.CustomerId,
            string.IsNullOrEmpty(comment.Customer.ProfilePictureUrl) ? null : comment.Customer.ProfilePictureUrl,
            comment.Content,
            comment.CreatedAt,
            comment.UpdatedAt,
            comment.LikesCount
        )).ToList();
    }
}