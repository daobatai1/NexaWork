namespace NexaWork.Application.Features.Client.Comment.Queries;

public record CommentQueryDTO(
    string CustomerName,
    Guid CommentId,
    Guid PostId,
    Guid CustomerId,
    string? CustomerProfilePictureUrl,
    string Content,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    int LikesCount);