using NexaWork.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexaWork.Application.Common.Interfaces.Repositories
{
    public interface IReactionRepository
    {
        void Add(Reaction reaction);
        void Remove(Reaction reaction);

        Task<Reaction?> GetPostReactionAsync(Guid customerId, Guid postId, CancellationToken cancellationToken);

        Task<Reaction?> GetCommentReactionAsync(Guid customerId, Guid commentId, CancellationToken cancellationToken);

        Task<List<Reaction>> GetReactionsForPostAsync(Guid postId, CancellationToken cancellationToken);

        Task<List<Reaction>> GetReactionsForCommentAsync(Guid commentId, CancellationToken cancellationToken);
    }
}
