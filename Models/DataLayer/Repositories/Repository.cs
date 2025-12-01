using Microsoft.EntityFrameworkCore;
using Airdnd.Models.DomainModels;

namespace Airdnd.Models.DataLayer.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected AirdndContext context { get; set; }
        private DbSet<T> dbSet { get; set; }

        public Repository(AirdndContext ctx)
        {
            context = ctx;
            dbSet = context.Set<T>();
        }

        public IEnumerable<T> List(QueryOptions<T> options)
        {
            IQueryable<T> query = dbSet;

            if (options.HasWhere)
                query = query.Where(options.Where);

            if (options.HasOrderBy)
                query = query.OrderBy(options.OrderBy);

            foreach (string include in options.Includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include.Trim());
            }

            return query.ToList();
        }

        public T? Get(int id) => dbSet.Find(id);
        public T? Get(string id) => dbSet.Find(id);

        public void Insert(T entity) => dbSet.Add(entity);
        public void Update(T entity) => dbSet.Update(entity);
        public void Delete(T entity) => dbSet.Remove(entity);
        public void Save() => context.SaveChanges();
    }
}