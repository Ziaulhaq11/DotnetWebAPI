using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync(); //Return Type
        Task<Region?> GetByIdAsync(Guid id); //it may or may not return Region if not found so thats why its optional
        Task<Region> CreateAsync(Region region);

        Task<Region?> UpdateAsync(Guid id, Region region);
        Task<Region?> DeleteAsync(Guid id);
    }
}
