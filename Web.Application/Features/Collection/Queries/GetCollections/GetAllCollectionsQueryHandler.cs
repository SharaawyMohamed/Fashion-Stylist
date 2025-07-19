using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Repositories;
using Web.Domain.Response;

namespace Web.Application.Features.Collection.Queries.GetCollections
{
	public class GetAllCollectionsQueryHandler : IRequestHandler<GetAllCollectionsQuery, BaseResponse>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetAllCollectionsQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public Task<BaseResponse> Handle(GetAllCollectionsQuery request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
