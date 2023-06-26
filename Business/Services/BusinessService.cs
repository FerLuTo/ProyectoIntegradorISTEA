using AccessData;
using AutoMapper;
using Business.Interfaces;
using Common.Exceptions;
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
                .Where(x => x.ActiveProfile != false && x.IsActive != false)
                .Select(x => _mapper.Map<UserBusinessResponse>(x));
            return business ;
        }

        public UserBusinessResponse GetBusinessById(int id)
        {
            var userBusiness = _context.UserBusinesses
               .Where(x => x.Id == id && x.IsActive != false)
               .Select(x => _mapper.Map<UserBusinessResponse>(x))
               .FirstOrDefault();

            if (userBusiness is null)
            {
                throw new KeyNotFoundException("User doesnt exists");
            }

            return userBusiness;
        }

        public async Task<UserBusinessResponse> Edit(int id, UserBusinessRequest model)
        {
            var userBusiness = await _context.UserBusinesses.FindAsync(id) ?? throw new NotFoundException("Account doesnt exists");

            userBusiness.ActiveProfile = true; 
            _mapper.Map(model, userBusiness);
            _context.UserBusinesses.Update(userBusiness);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserBusinessResponse>(userBusiness);
        }

    }
}
