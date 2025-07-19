using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entites;
using Web.Domain.Repositories;
using Web.Infrastructure.Data;

namespace Web.Infrastructure.Repositories
{
	public class BaseRepository<TKey,TEntity> : IBaseRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
	{
		private readonly AppDbContext _context;

		public BaseRepository(AppDbContext context)
		{
			_context = context;
			_entities = context.Set<TEntity>();
		}

		private readonly DbSet<TEntity> _entities;
		public async Task<IEnumerable<TEntity>> GetAllAsync()
		=> await _entities.ToListAsync();
	}
}
