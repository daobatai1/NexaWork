using NexaWork.Application.Common.Interfaces.Repositories;
using NexaWork.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexaWork.Infrastructure.Persistence.Repositories
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly NexaWorkDbContext _context;
        public ReactionRepository(NexaWorkDbContext context)
        {
            _context = context;
        }
        public void Add(Reaction reaction)
        {
            throw new NotImplementedException();
        }

        public Task<Reaction?> GetCommentReactionAsync(Guid customerId, Guid commentId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Reaction?> GetPostReactionAsync(Guid customerId, Guid postId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Reaction>> GetReactionsForCommentAsync(Guid commentId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Reaction>> GetReactionsForPostAsync(Guid postId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Remove(Reaction reaction)
        {
            throw new NotImplementedException();
        }
    }
}
