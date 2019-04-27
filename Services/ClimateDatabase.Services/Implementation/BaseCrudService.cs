namespace ClimateDatabase.Services.Implementation
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ClimateDatabase.Data.Common.Models;
    using ClimateDatabase.Data.Common.Repositories;
    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Services.Contracts;

    public class BaseCrudService<T> : IBaseCrudService<T>
        where T : class
    {
        private IRepository<T> data;

        public BaseCrudService(IRepository<T> data)
        {
            this.data = data;
        }

        public IQueryable<T> GetAll()
        {
            var result = this.data.All();
            
            return result;
        }

        public async Task<T> Get(params object[] id)
        {
            var entity = await this.data.GetByIdAsync(id);
            
            return entity;
        }

        public async Task<T> Create(T entity)
        {
            this.data.Add(entity);
            await this.data.SaveChangesAsync();
            
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            this.data.Update(entity);
            await this.data.SaveChangesAsync();
            
            return entity;
        }
    }
}