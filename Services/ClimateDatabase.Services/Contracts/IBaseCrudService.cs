namespace ClimateDatabase.Services.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;
    using ClimateDatabase.Data.Common.Models;

    public interface IBaseCrudService<T>
    {
        IQueryable<T> GetAll();

        //IQueryable<T> GetAllWithDeleted();

        Task<T> Get(params object[] id);

        Task<T> Create(T entity);

        Task<T> Update(T entity);

        //Task<bool> Delete (object id);

        //Task<bool> Delete(IQueryable<T> id);

        //Task<bool> Restore(object id);

        //Task<bool> Restore(IQueryable<T> id);

        //Task<bool> Exists(object key);
    }
}