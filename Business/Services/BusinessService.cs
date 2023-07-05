using AccessData;
using AutoMapper;
using Business.Interfaces;
using Common.Exceptions;
using Common.Helper;
using Entities.Models;
using Entities.ViewModels.Request;
using Entities.ViewModels.Response;

namespace Business.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public BusinessService(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IEnumerable<UserBusinessResponse> GetBusiness()
        {
            var business = _context.UserBusinesses
                .Where(x => x.ActiveProfile && x.IsActive)
                .Select(x => _mapper.Map<UserBusinessResponse>(x));
                //.OrderBy(x => x.FantasyName);

            return business ;
        }

        public UserBusinessResponse GetBusinessById(int id)
        {
            var userBusiness = _context.UserBusinesses
               .Where(x => x.Id == id && x.IsActive != false)
               .Select(x => _mapper.Map<UserBusinessResponse>(x))
               .FirstOrDefault();

            return userBusiness is null ? throw new KeyNotFoundException("User doesnt exists") : userBusiness;
        }

        public async Task<UserBusinessResponse> Edit(int id, UserBusinessRequest model)
        {
            var userBusiness = await _context.UserBusinesses.FindAsync(id) ?? throw new KeyNotFoundException("Account doesnt exists");

            if(userBusiness.FantasyName is null || userBusiness.BusinessName is null || userBusiness.Slogan is null 
               || userBusiness.Description is null || userBusiness.Address is null || userBusiness.Location is null
               || userBusiness.Cuit is null || userBusiness.Alias is null || userBusiness.Web is null)
                
            {
                throw new AppException("The fields must have data");
            }
            /*
            if(userBusiness.PostalCode <= 0)
            {
                throw new AppException($"The field {userBusiness.PostalCode} must have data");
            }
            */
            userBusiness.ActiveProfile = true; 
            _mapper.Map(model, userBusiness);
            _context.UserBusinesses.Update(userBusiness);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserBusinessResponse>(userBusiness);
        }

    }
}
