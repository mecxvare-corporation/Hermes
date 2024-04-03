﻿using PostService.Domain.Entities;

using System.Linq.Expressions;

namespace PostService.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        // TODO: Add other needed method signatures
        public Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);
    }
}
