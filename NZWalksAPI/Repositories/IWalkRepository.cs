using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public interface IWalkRepository
    {
        //Task type is a return type
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllWalks(string? filterOn = null, string? filterQuery = null);

        Task<Walk?> GetWalkByID(Guid ID);

        Task<Walk?> UpdateAsync(Guid id, Walk walk);

        Task<Walk?>DeleteAsync(Guid id);
    }
}
