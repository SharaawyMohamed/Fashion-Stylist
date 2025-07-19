using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entites;
using Web.Domain.Repositories;
using Web.Infrastructure.Data;

namespace Web.Infrastructure.Repositories
{
	public class UnitOfWrok : IUnitOfWork
	{
		private readonly AppDbContext _context;
		private readonly ConcurrentDictionary<string, object> _repo = new();

		public UnitOfWrok(AppDbContext context)
		{
			_context = context;
		}

		public IBaseRepository<TKey, TEntity> Repository<TKey, TEntity>() where TEntity : BaseEntity<TKey>
		{
			var typeName = typeof(TEntity).Name;

			if (!_repo.ContainsKey(typeName))
			{
				var repositoryInstance = new BaseRepository<TKey, TEntity>(_context);
				_repo[typeName] = repositoryInstance;
			}

			return (BaseRepository<TKey, TEntity>)_repo[typeName];
		}

		public async ValueTask DisposeAsync()
			=> await _context.DisposeAsync();

		public async Task<int> SaveChangesAsync()
			=> await _context.SaveChangesAsync();

	}
}
