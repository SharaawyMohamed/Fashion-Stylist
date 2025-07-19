using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.Features.Collection.Queries.GetCollections
{
	public record GetAllCollectionItemsQueryDto(Guid id,string title,string pictureUrl,string description);
}
