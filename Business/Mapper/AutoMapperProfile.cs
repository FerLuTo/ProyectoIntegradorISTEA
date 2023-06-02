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

            CreateMap<Account, UserBusinessRequest>();

            #endregion

            #region UserBusiness
            CreateMap<UserBusiness, UserBusinessResponse>();

            CreateMap<UserBusinessRequest, UserBusiness>();

            #endregion

            #region Product

            CreateMap<Product, ProductResponse>();

            CreateMap<ProductRequest, Product>();

            #endregion

            #region Sale
            CreateMap<Sale, SaleResponse>();

            CreateMap<SaleRequest, Sale>();
            #endregion

            #region SaleDetail
            CreateMap<SaleDetail, SaleDetailResponse>();

            #endregion
        }

    }
}
