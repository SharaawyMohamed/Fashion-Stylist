using AutoMapper;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.Features.User.Queries.GetUser;
using Web.Domain.DTOs.ProductDTO;
using Web.Domain.DTOs.UserDTO;
using Web.Domain.Entites;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.Application.Mapping
{
	public class MappingProfile : IRegister
    {

		public void Register(TypeAdapterConfig config)
		{

            config.NewConfig<AppUser, UserDto>()
              .Map(dest => dest.UserName, src => src.FullName)
              .Map(dest => dest.profilePictureUrl, src => src.ProfilePicture)
              .Map(dest => dest.Id, src => src.Id)
              .Map(dest => dest.Email, src => src.Email);

            // Review → ReviewDto
            config.NewConfig<Review, ReviewDto>()
                  .Map(dest => dest.User, src => src.appUser)
                  .Map(dest => dest.Rate, src => src.rate)
                  .Map(dest => dest.Comment, src => src.comment)
                  .Map(dest => dest.Id, src => src.id)
                  .Map(dest => dest.User, src => src.appUser);
                  

            // Product → ProductDto
            config.NewConfig<Product, ProductDto>()
              .Map(dest => dest.Id ,src => src.id)
              .Map(dest => dest.Title, src => src.title)
              .Map(dest => dest.PictureUrl, src => src.pictureUrl)
              .Map(dest => dest.BasePrice, src => src.basePrice)
              .Map(dest => dest.DiscountedPrice, src => src.discountedPrice)
              .Map(dest => dest.Rate, src => src.rate)
              .Map(dest => dest.Colors, src => src.colors)
              .Map(dest => dest.Sizes, src => src.sizes)
              .Map(dest => dest.Brand, src => src.brand)
              .Map(dest => dest.IsOffred, src => src.isOffred)
              .Map(dest => dest.ProductType, src => src.ProductType)
              .Map(dest => dest.CategoryId, src => src.categoryId)
              .Map(dest => dest.Reviews, src => src.reviews);

            config.NewConfig<AppUser, GetUserQueryDto>();
		}
	}
}
