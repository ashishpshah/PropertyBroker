using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Broker.Infra
{
	public interface IRepositoryBase<T> where T : class
	{
		IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
		IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
		bool Any(Expression<Func<T, bool>> expression);
		T Add(T entity);
		void Add(List<T> entity);
		void Update(T entity);
		void Delete(T entity);
	}


	public class RepositoryBase<T>(DataContext context) : IRepositoryBase<T> where T : class
	{
		private readonly DataContext _context = context;
		public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
		{
			IQueryable<T> query = _context.Set<T>();

			foreach (var include in includes) query = query.Include(include);

			return query.AsNoTracking();
		}
		public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
		{
			IQueryable<T> query = _context.Set<T>().Where(expression);

			foreach (var include in includes) query = query.Include(include);

			return query.AsNoTracking();
		}
		public bool Any(Expression<Func<T, bool>> expression) => _context.Set<T>().Any(expression);
		public T Add(T entity) { _context.Set<T>().Add(entity); _context.Set<T>().Entry(entity).Reload(); _context.SaveChanges(); _context.Set<T>().Entry(entity).State = EntityState.Detached; return entity; }
		public void Add(List<T> entity) { _context.Set<T>().AddRange(entity); _context.SaveChanges(); }
		public void Update(T entity) { _context.Set<T>().Entry(entity).State = EntityState.Modified; _context.SaveChanges(); _context.Set<T>().Entry(entity).State = EntityState.Detached; }
		public void Delete(T entity) { _context.Set<T>().Entry(entity).State = EntityState.Deleted; _context.SaveChanges(); _context.Set<T>().Entry(entity).State = EntityState.Detached; }
	}


	public interface IRepositoryWrapper
	{
		IRepositoryBase<T> Using<T>() where T : class;
		IDbContextTransaction BeginTransaction();
	}

	public class RepositoryWrapper(DataContext context) : IRepositoryWrapper
	{
		private readonly DataContext _context = context;
		public IRepositoryBase<T> Using<T>() where T : class => new RepositoryBase<T>(_context);
		public IDbContextTransaction BeginTransaction() { return _context.Database.BeginTransaction(); }

	}
}
