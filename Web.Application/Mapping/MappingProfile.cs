using AutoMapper;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.Features.User.Queries.GetUser;
using Web.Domain.Entites;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.Application.Mapping
{
	public class MappingProfile : IRegister
    {

		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<AppUser, GetUserQueryDto>();
		}
	}
}
