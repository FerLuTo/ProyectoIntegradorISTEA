using AccessData;
using AutoMapper;
using Business.Interfaces;
using Common.Exceptions;
using Common.Helper;
using Entities.Enum;
using Entities.Models;
using Entities.ViewModels.Request;
using Entities.ViewModels.Response;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IAzureBlobStorageService _azureBlobStorageService;
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public ProductService(IAzureBlobStorageService azureBlobStorageService, AppDBContext context, IMapper mapper)
        {
            _azureBlobStorageService = azureBlobStorageService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponse>> GetProductsByUserBusiness(int idUserBusiness)
        {

            var products = await _context.Products
            .Where(x => x.UserBusiness.Id == idUserBusiness && x.IsActive !=false && x.Stock > 0)
            .ToListAsync();

            return _mapper.Map<IList<ProductResponse>>(products);


        }

        public ProductResponse GetProduct(int id)
        {
            var product = _context.Products
                .Where(x => x.Id == id && x.IsActive != false)
                .Select(x => _mapper.Map<ProductResponse>(x))
                .FirstOrDefault();

            return product;
        }

        public async Task<ProductResponse> Create(ProductRequest model)
        {
            var business = await _context.UserBusinesses.FindAsync(model.UserBusinessId); 

            if(business is null || business.IsActive == false)
                throw new KeyNotFoundException("El usuario no existe");

            if(model.Stock <= 0 || model.Price <= 0)
            {
                throw new AppException("Los campos stock y precio son obligatorios");
            }

            var product = _mapper.Map<Product>(model);

            if (model.Image != null)
            {
                product.ImagePath = await _azureBlobStorageService.UploadAsync(model.Image, ContainerEnum.IMAGES);
            }


            product.IsActive = true;    
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            var productResponse = _mapper.Map<ProductResponse>(product);
            productResponse.ImageUrl = "https://hungryheroesstorage.blob.core.windows.net/images/" + product.ImagePath;
            return productResponse;
        }

        public async Task<ProductResponse> Edit(int id, ProductRequest model)
        {
            var product = await _context.Products.FindAsync(id);

            _mapper.Map(model, product);

            if (model.Image != null)
            {
                product.ImagePath = await _azureBlobStorageService.UploadAsync(model.Image, ContainerEnum.IMAGES);
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            var productResponse = _mapper.Map<ProductResponse>(product);
            productResponse.ImageUrl = "https://hungryheroesstorage.blob.core.windows.net/images/" + product.ImagePath;

            return productResponse;
        }

        public  void Delete(int id)
        {
            var product = _context.Products.Find(id);

            product.IsActive = false;
            _context.Products.Update(product);
            _context.SaveChanges();
        }

    }
}
