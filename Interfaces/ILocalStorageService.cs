namespace BlazorApp1.Interfaces
{
    public interface ILocalStorageService
    {
        Task<bool> Save();
        Task<bool> Load();
    }
}
