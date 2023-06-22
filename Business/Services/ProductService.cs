using AccessData;
using AutoMapper;
using Business.Interfaces;
using Common.Exceptions;
using Entities.Enum;
using Entities.Models;
using Entities.ViewModels.Request;
using Entities.ViewModels.Response;

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

        public IEnumerable<ProductResponse> GetProductsByUserBusiness(int idUserBusiness)
        {
            var products = _context.Products.Any(x => x.UserBusiness.Id == idUserBusiness);
            return _mapper.Map<IList<ProductResponse>>(products);

        }

        public ProductResponse GetProduct(int id)
        {
            var product = _context.Products.Find(id);

            return product is null || product.IsActive is false ? throw new NotFoundException("Product doesnt exists") : _mapper.Map<ProductResponse>(product);
        }

        public async Task<ProductResponse> Create(ProductRequest model)
        {
            _ = await _context.UserBusinesses.FindAsync(model.UserBusinessId) ?? throw new NotFoundException("User not found");


            var product = _mapper.Map<Product>(model);
/*
            if (model.Image != null)
            {
                product.ImagePath = await _azureBlobStorageService.UploadAsync(model.Image, ContainerEnum.IMAGES);
            }
*/          product.IsActive = true;    
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductResponse>(product);
        }

        public async Task<ProductResponse> Edit(int id, ProductRequest model)
        {
            var product = await _context.Products.FindAsync(id);
            if(product is null || product.IsActive is false)
            {
                throw new NotFoundException("Product doesnt exists");
            }   

            _mapper.Map(model, product);
/*
            if (model.Image != null)
            {
                product.ImagePath = await _azureBlobStorageService.UploadAsync(model.Image, ContainerEnum.IMAGES);
            }
*/
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductResponse>(product);
        }

        public async void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product is null || product.IsActive is false)
            {
                throw new NotFoundException("Product doesnt exists");
            }
            /*
            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    await _azureBlobStorageService.DeleteAsync(ContainerEnum.IMAGES, product.ImagePath);
                }

            }
            */
            product.IsActive = false;
            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }
}
