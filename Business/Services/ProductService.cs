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

        /// <summary>
        /// Method to get all the products by userBusiness
        /// only used by Business
        /// </summary>
        /// <param name="idUserBusiness"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public IEnumerable<ProductResponse> GetProductsByUserBusiness(int idUserBusiness)
        {
            var userbusiness = _context.UserBusinesses.Find(idUserBusiness);
            var products = _context.Products;
            return userbusiness is null ? throw new NotFoundException("User doesnt exists") : _mapper.Map<IList<ProductResponse>>(products);

        }

        /// <summary>
        /// Method to get all the products of all the userBusiness
        /// only used by Admin
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductResponse> GetProducts()
        {
            var products = _context.Products;

            return _mapper.Map<IList<ProductResponse>>(products);
        }

        /// <summary>
        /// Method to get products by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public ProductResponse GetProduct(int id)
        {
            var product = _context.Products.Find(id);
            return product is null ? throw new NotFoundException("Product doesnt exists") : _mapper.Map<ProductResponse>(product);
        }

        /// <summary>
        /// Method to create product (box)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<ProductResponse> Create(ProductRequest model)
        {
            _ = await _context.UserBusinesses.FindAsync(model.UserBusinessId) ?? throw new NotFoundException("User not found");


            var product = _mapper.Map<Product>(model);

            if (model.Image != null)
            {
                product.ImagePath = await _azureBlobStorageService.UploadAsync(model.Image, ContainerEnum.IMAGES);
            }

            if (model.TechnicalDataSheet != null)
            {
                product.TechnicalDataSheet = await _azureBlobStorageService.UploadAsync(model.TechnicalDataSheet, ContainerEnum.DOCUMENTS);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductResponse>(product);
        }

        /// <summary>
        /// Method to edit product, finding by 
        /// product id and updating model
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<ProductResponse> Edit(int id, ProductRequest model)
        {
            var product = await _context.Products.FindAsync(id) ?? throw new NotFoundException("Product doesnt exists");

            _mapper.Map(model, product);

            if (model.Image != null)
            {
                product.ImagePath = await _azureBlobStorageService.UploadAsync(model.Image, ContainerEnum.IMAGES);
            }

            if (model.TechnicalDataSheet != null)
            {
                product.TechnicalDataSheet = await _azureBlobStorageService.UploadAsync(model.Image, ContainerEnum.IMAGES);
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductResponse>(product);
        }

        /// <summary>
        /// Method to delete product physically, finding for 
        /// product id.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="NotFoundException"></exception>
        public async void Delete(int id)
        {
            var product = _context.Products.Find(id) ?? throw new NotFoundException("Product doesnt exists");

            if(product != null)
            { 
            if (!string.IsNullOrEmpty(product.ImagePath))
            {
                await _azureBlobStorageService.DeleteAsync(ContainerEnum.IMAGES, product.ImagePath);
            }

            if (!string.IsNullOrEmpty(product.TechnicalDataSheet))
            {
                await _azureBlobStorageService.DeleteAsync(ContainerEnum.IMAGES, product.TechnicalDataSheet);
            }
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        /// <summary>
        /// Method to delete product logically, finding for 
        /// product id.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="NotFoundException"></exception>
       /* public Task<bool> IsActive(int id)
        {
            var product = _context.Products.Find(id) ?? throw new NotFoundException("Product doesnt exists");

            _context.Products.Update(product);
            _context.SaveChanges();
            return Ok("Product succesfully deleted");
        }
       */
    }
}
