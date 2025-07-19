namespace NationalParkWebApp.Repository.IRepository
{
    public interface IRepository<T>where T : class
    {
        Task<T> GetAsync(string url, int id);//find
        Task<IEnumerable<T>> GetAllAsync(string url);//Display
        Task<bool> CreateAsync(string url, T ObjToCreate);//Create
        Task<bool> UpdateAsync(string url, T ObjToUpdate);//Update
        Task<bool> DeleteAsync(string url, int id);//Delete
    }
}
