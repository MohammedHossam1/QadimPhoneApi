using Microsoft.AspNetCore.Mvc;
using System.Net;
using Talabat.APIs.Errors;
using Talabat.Repositories.Data;

namespace Talabat.APIs.Controllers
{
    public class BuggyController : BaseAPIController
    {
        private readonly StoreDBContext _context;

        public BuggyController(StoreDBContext context)
        {
            _context = context;
        }
        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = _context.Products.Find(1000);
            if (product is null)
                return NotFound(new ApiResponse(404)) ;
            return Ok(product);
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var product = _context.Products.Find(1000);
            var res = product.ToString();
            return Ok(res);
        }
        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return NotFound(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int? id)
        {
            return NotFound();
        }

        [HttpGet("unauthorized")]
        public ActionResult GetUnauthorized()
        {
            return Unauthorized(new ApiResponse(401));
        }



    }
}
