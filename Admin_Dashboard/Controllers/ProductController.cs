using Admin_Dashboard.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Admin_Dashboard.Helpers;

namespace Admin_Dashboard.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            // Get All Products

            var products = await _unitOfWork.Repositories<Product>().GetAllAsync();

            var mappedProduct = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(products);


            return View(mappedProduct);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
                if (model.Image != null)
                {
                    model.PictureUrl = PictureSetting.UploadFile(model.Image, "products");
                }

                else
                    model.PictureUrl = "images/products/hat-react2.png";

                var mappedProduct = _mapper.Map<ProductViewModel, Product>(model);

                await _unitOfWork.Repositories<Product>().AddAsync(mappedProduct);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction("Index");


        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.Repositories<Product>().GetAsync(id);

            var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);

            return View(mappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {
            if (id != model.Id)
                return NotFound();

                if (model.Image != null)
                {
                    if (model.PictureUrl != null)
                    {
                        PictureSetting.DeleteFile(model.PictureUrl, "products");
                        model.PictureUrl = PictureSetting.UploadFile(model.Image, "products");
                    }

                    else
                        model.PictureUrl = PictureSetting.UploadFile(model.Image, "products");

                    var mappedProduct = _mapper.Map<ProductViewModel, Product>(model);

                    _unitOfWork.Repositories<Product>().Update(mappedProduct);

                    var result = await _unitOfWork.CompleteAsync();

                    if (result > 0)
                        return RedirectToAction("Index");
            }

            return View(model);

        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Repositories<Product>().GetAsync(id);

            var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);

            return View(mappedProduct);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(int id, ProductViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            try
            {
                var product = await _unitOfWork.Repositories<Product>().GetAsync(id);

                if (product.PictureUrl != null)
                    PictureSetting.DeleteFile(product.PictureUrl, "products");

                _unitOfWork.Repositories<Product>().Delete(product);

                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            catch (System.Exception)
            {

                return View(model);
            }
        }


    }
}
