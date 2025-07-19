using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.Features.Collection.Queries.GetCollections
{
	public class GetAllCollectionsQueryDto(Guid id,string title,string subTitle,string description,decimal price, string pictureUrl,IReadOnlyList<GetAllCollectionItemsQueryDto> items );
	
}
