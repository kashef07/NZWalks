﻿using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkDifficultiesRepository
    {
        Task<IEnumerable<WalkDifficulty>> GetAllAsync();
        Task<WalkDifficulty> GetAsync(Guid id);
        Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty);
        Task<WalkDifficulty> UpdateAsync(Guid id,WalkDifficulty walkDifficulty);
        Task<WalkDifficulty> DeleteAsync(Guid id);  
    }
}
