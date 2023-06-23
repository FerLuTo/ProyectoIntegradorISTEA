using AutoMapper;
using Entities.Models;
using Entities.ViewModels.Request;
using Entities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            #region Account
            CreateMap<Account, AccountResponse>();

            CreateMap<Account, AuthenticateResponse>();

            CreateMap<RegisterRequest, Account>();

            CreateMap<ChangePasswordRequest, Account>();

            CreateMap<Account, UserBusinessRequest>();

            CreateMap<UserBusiness, AuthenticateResponse>()
                .ForMember(x => x.UserBusinessId, map => map.MapFrom(src => src.Id));

            CreateMap<UserClient, AuthenticateResponse>()
                .ForMember(x => x.UserClientId, map => map.MapFrom(src => src.Id));

            #endregion

            #region UserBusiness
            CreateMap<UserBusiness, UserBusinessResponse>()
                .ForMember(x => x.UserBusinessId, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.FantasyName, map => map.MapFrom(src => src.FantasyName))
                .ForMember(x => x.BusinessName, map => map.MapFrom(src => src.BusinessName))
                .ForMember(x => x.Slogan, map => map.MapFrom(src => src.Slogan))
                .ForMember(x => x.Description, map => map.MapFrom(src => src.Description))
                .ForMember(x => x.Address, map => map.MapFrom(src => src.Address))
                .ForMember(x => x.PostalCode, map => map.MapFrom(src => src.PostalCode))
                .ForMember(x => x.Location, map => map.MapFrom(src => src.Location))
                .ForMember(x => x.Cuit, map => map.MapFrom(src => src.Cuit))
                .ForMember(x => x.Alias, map => map.MapFrom(src => src.Alias))
                .ForMember(x => x.Web, map => map.MapFrom(src => src.Web))
                .ForMember(x => x.ActiveProfile, map => map.MapFrom(src => src.ActiveProfile));

            CreateMap<UserBusinessRequest, UserBusiness>();

            #endregion


            #region Product

            CreateMap<Product, ProductResponse>();

            CreateMap<ProductRequest, Product>();

            #endregion

            #region Sale
            CreateMap<Sale, SaleResponse>();

            CreateMap<SaleRequest, Sale>();

            CreateMap<Product, SaleResponse>()
                .ForMember(x => x.BoxName, map => map.MapFrom(src => src.Name));

            CreateMap<UserBusiness, SaleResponse>()
                .ForMember(x => x.FantasyName, map => map.MapFrom(src => src.FantasyName));

            CreateMap<Account, SaleResponse>()
                .ForMember(x => x.Email, map => map.MapFrom(src => src.Email));
            
            #endregion

        }

    }
}
