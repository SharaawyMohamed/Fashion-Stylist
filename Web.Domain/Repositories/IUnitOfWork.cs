using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entites;

namespace Web.Domain.Repositories
{
	public interface IUnitOfWork : IAsyncDisposable
	{
		IBaseRepository<TKey, TEntity> Repository<TKey, TEntity>() where TEntity : BaseEntity<TKey>;
		Task<int> SaveChangesAsync();
	}
}
