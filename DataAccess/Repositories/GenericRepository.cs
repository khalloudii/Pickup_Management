using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class GenericRepository<T> where T : class
    {
        private readonly PickupContext _context;
        private readonly DbSet<T> dbSet;
        public GenericRepository(PickupContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }


        public async Task<T> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public IList<T> ListAll()
        {
            return dbSet.ToList();
        }

        public async Task<IList<T>> ListAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public IQueryable<T> ListIQueryableAsync()
        {
            return dbSet;
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }


    }
}
