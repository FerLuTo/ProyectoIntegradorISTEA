﻿using AccessData;
using AutoMapper;
using Business.Interfaces;
using Common.Exceptions;
using Common.Helper;
using Entities.Enum;
using Entities.Models;
using Entities.ViewModels.Request;
using Entities.ViewModels.Response;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly IAzureBlobStorageService _azureBlobStorageService;
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public BusinessService(IAzureBlobStorageService azureBlobStorageService, AppDBContext context, IMapper mapper)
        {
            _azureBlobStorageService = azureBlobStorageService;
            _context = context;
            _mapper = mapper;
        }
        public IEnumerable<UserBusinessResponse> GetBusiness()
        {
            var business = _context.UserBusinesses
                .Where(x => x.ActiveProfile && x.IsActive)
                .OrderBy(x => x.FantasyName)
                .Select(x => _mapper.Map<UserBusinessResponse>(x));
                

            return business ;
        }

        public UserBusinessResponse GetBusinessById(int id)
        {
            var userBusiness = _context.UserBusinesses
               .Where(x => x.Id == id && x.IsActive != false)
               .Select(x => _mapper.Map<UserBusinessResponse>(x))
               .FirstOrDefault();

            var userBusinessImage = _context.UserBusinesses
               .Where(x => x.Id == id && x.IsActive != false)
               .Select(x => _mapper.Map<UserBusiness>(x))
               .FirstOrDefault();

            userBusiness.ImageUrl = "https://hungryheroesstorage.blob.core.windows.net/images/" + userBusinessImage.ImagePath;


            return userBusiness is null ? throw new KeyNotFoundException("User doesnt exists") : userBusiness;
        }

        public async Task<UserBusinessResponse> Edit(int id, UserBusinessRequest model)
        {
            var userBusiness = await _context.UserBusinesses.FindAsync(id) ?? throw new KeyNotFoundException("Account doesnt exists");


            userBusiness.ActiveProfile = true; 
            _mapper.Map(model, userBusiness);

            if (model.Image != null)
            {
                userBusiness.ImagePath = await _azureBlobStorageService.UploadAsync(model.Image, ContainerEnum.IMAGES);
            }

            _context.UserBusinesses.Update(userBusiness);
            await _context.SaveChangesAsync();
            var response = _mapper.Map<UserBusinessResponse>(userBusiness);
            response.ImageUrl = "https://hungryheroesstorage.blob.core.windows.net/images/" + userBusiness.ImagePath;
            return response;
        }

        public IEnumerable<UserBusinessResponse> FilterFantasyName(string fantasyName)
        {
            var filterFantasyName = _context.UserBusinesses
                .Where(b => b.FantasyName.Contains(fantasyName))
                .OrderBy(b => b.FantasyName)
                .Select(b => _mapper.Map<UserBusinessResponse>(b));

            return filterFantasyName;
            
            //TODO: si no encuentra coincidencia mensaje: "Sin coincidencias."

            /*var filterFantasyName = await _context.UserBusinesses.FirstOrDefaultAsync(b => b.FantasyName.StartsWith(fantasyName));
            if (fantasyName is null)
                throw new KeyNotFoundException("No encontramos el negocio que buscás");
            
            return (IEnumerable<UserBusinessResponse>)_mapper.Map<UserBusinessResponse>(filterFantasyName);*/
        }

        public IEnumerable<UserBusinessResponse> FilterLocation(string location)
        {
            var filterLocation = _context.UserBusinesses
                .Where(b => b.Location.Contains(location))
                .OrderBy(b => b.Location)
                .Select(b => _mapper.Map<UserBusinessResponse>(b));

            return filterLocation;

            //TODO: si no encuentra coincidencia mensaje: "Sin coincidencias."
        }
    }
}
