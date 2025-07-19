using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Response;

namespace Web.Application.Features.User.Queries.GetUser
{
	public record GetUserQuery() : IRequest<BaseResponse>;
}
