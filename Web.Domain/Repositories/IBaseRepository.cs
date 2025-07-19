using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entites;

namespace Web.Domain.Repositories
{
	public interface IBaseRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
	{
		Task<IEnumerable<TEntity>> GetAllAsync();

	}
}
