using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification.ProductSpecs
{
    public class ProductWithCountSpec:BaseSpecification<Product>
    {
        public ProductWithCountSpec(ProductSpecParams productSpec) :
        base(p =>(!productSpec.CategoryId.HasValue || p.ProductTypeId == productSpec.CategoryId))
        {

        }
    }
}
