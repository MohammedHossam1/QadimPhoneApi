using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.RepositoriesInterfaces;
using Talabat.Core.Specification;
using Talabat.Core.Specification.ProductSpecs;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : BaseAPIController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IGenericRepository<Product> productRepo, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetAllProducts([FromQuery] ProductSpecParams productSpec)
        {
            //var products = await _productRepo.GetAllAsync();
            var spec = new ProductWithSpecsBrandAndCategory(productSpec);
            var products = await _productRepo.GetAllWithSpecAsync(spec);
            var res = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var countSpec = new ProductWithCountSpec(productSpec);
            var count = await _productRepo.GetCountAsync(countSpec);

            return Ok(new Pagination<ProductToReturnDto>(productSpec.PageIndex, productSpec.PageSize, count, res));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            //var product = await _productRepo.GetAsync(id);
            var spec = new ProductWithSpecsBrandAndCategory(id);

            var product = await _productRepo.GetWithSpecAsync(spec);
            if (product is null)
                return NotFound(new { Message = "Not Found", StatusCode = 404 });
            var res = _mapper.Map<Product, ProductToReturnDto>(product);

            return Ok(res);
        }


        [HttpGet("types")]
        public async Task<ActionResult<ProductType>> GetTypes()
        {
            var types = await _unitOfWork.Repositories<ProductType>().GetAllAsync();
            return Ok(types);
        }


        //[HttpPost]
        //public async Task<ActionResult<ProductToReturnDto>> PostProduct([FromForm] PostProductDto productDto)
        //{
        //    if (productDto.Image != null)
        //    {
        //        productDto.PictureUrl = PictureSetting.UploadFile(productDto.Image, "products");
        //    }
        //    else
        //    {
        //        productDto.PictureUrl = "images/products/hat-react2.png";
        //    }

        //    Mapping DTO to Entity
        //    var product = _mapper.Map<PostProductDto, Product>(productDto);

        //    Adding the product to the repository
        //    await _productRepo.AddAsync(product);

        //    Saving changes to the database
        //   var result = await _unitOfWork.CompleteAsync();

        //    if (result <= 0)
        //        return BadRequest(new { Message = "Failed to create product" });

        //    Fetch the product with included ProductType
        //    var productFromDb = await _productRepo.GetAsync(product.Id);

        //    Mapping the product entity back to a DTO
        //    var productToReturn = _mapper.Map<Product, ProductToReturnDto>(productFromDb);

        //    Returning the created product
        //    return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productToReturn);
        //}






    }

}
