namespace ProjectServices.Services.Interfaces
{
    public interface IMenuService
    {
        Task<IEnumerable<object>> GetMenuAsync(string userId);
    }
}