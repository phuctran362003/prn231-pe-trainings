using Microsoft.EntityFrameworkCore;
using Repository.Entities;

namespace Repository
{
    public class DataAccessObject<T> where T : class
    {
        protected Spring2025productinventorydbContext Context;

        // Default constructor, initializes the DbContext if null
        public DataAccessObject()
        {
            Context = new Spring2025productinventorydbContext();
        }

        // Constructor that accepts a DbContext
        public DataAccessObject(Spring2025productinventorydbContext context)
        {
            Context = context;
        }

        // Synchronous method to retrieve all entities
        public List<T> GetAll()
        {
            return Context.Set<T>().ToList();
        }

        // Asynchronous method to retrieve all entities
        public async Task<List<T>> GetAllAsync()
        {
            return await Context.Set<T>().ToListAsync();
        }

        // Synchronous method to create a new entity
        public void Create(T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        // Asynchronous method to create a new entity
        public async Task<int> CreateAsync(T entity)
        {
            Context.Add(entity);
            return await Context.SaveChangesAsync();
        }

        // Synchronous method to update an entity
        public void Update(T entity)
        {
            var tracker = Context.Attach(entity);
            tracker.State = EntityState.Modified;
            Context.SaveChanges();
        }

        // Asynchronous method to update an entity
        public async Task<int> UpdateAsync(T entity)
        {
            var tracker = Context.Attach(entity);
            tracker.State = EntityState.Modified;
            return await Context.SaveChangesAsync();
        }

        // Synchronous method to remove an entity
        public bool Remove(T entity)
        {
            Context.Remove(entity);
            Context.SaveChanges();
            return true;
        }

        // Asynchronous method to remove an entity
        public async Task<bool> RemoveAsync(T entity)
        {
            Context.Remove(entity);
            await Context.SaveChangesAsync();
            return true;
        }

        // Retrieve entity by integer ID (synchronously)
        public T GetById(int id)
        {
            return Context.Set<T>().Find(id);
        }

        // Retrieve entity by integer ID (asynchronously)
        public async Task<T> GetByIdAsync(int id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        // Retrieve entity by string ID (synchronously)
        public T GetById(string code)
        {
            return Context.Set<T>().Find(code);
        }

        // Retrieve entity by string ID (asynchronously)
        public async Task<T> GetByIdAsync(string code)
        {
            return await Context.Set<T>().FindAsync(code);
        }

        // Retrieve entity by Guid (synchronously)
        public T GetById(Guid code)
        {
            return Context.Set<T>().Find(code);
        }

        // Retrieve entity by Guid (asynchronously)
        public async Task<T> GetByIdAsync(Guid code)
        {
            return await Context.Set<T>().FindAsync(code);
        }

        // Prepare an entity for creation (no save)
        public void PrepareCreate(T entity)
        {
            Context.Add(entity);
        }

        // Prepare an entity for update (no save)
        public void PrepareUpdate(T entity)
        {
            var tracker = Context.Attach(entity);
            tracker.State = EntityState.Modified;
        }

        // Prepare an entity for removal (no save)
        public void PrepareRemove(T entity)
        {
            Context.Remove(entity);
        }

        // Save changes synchronously
        public int Save()
        {
            return Context.SaveChanges();
        }

        // Save changes asynchronously
        public async Task<int> SaveAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
