using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification.ProductSpecs
{
    public class ProductWithSpecsBrandAndCategory:BaseSpecification<Product>
    {
        public ProductWithSpecsBrandAndCategory(ProductSpecParams productSpec) :
            base(p=>
            (string.IsNullOrEmpty(productSpec.Search)||p.Name.ToLower().Contains(productSpec.Search))
            &&
            (!productSpec.CategoryId.HasValue || p.ProductTypeId== productSpec.CategoryId))
        {
            Includat.Add(p => p.ProductType);

            if (!string.IsNullOrEmpty(productSpec.Sort)) {
            switch (productSpec.Sort) {
                    case "priceAsc":
                        addOrderBy(p=>p.Price); break;
                    case "priceDesc":
                        addOrderByDesc ( p => p.Price); break;
                    default :
                        addOrderBy(p => p.Name); break;
                }
            }
            else
            {
                addOrderBy(p => p.Name); 

            }

          //  addPagination((productSpec.PageSize) * (productSpec.PageIndex - 1), productSpec.PageSize);
            addPagination( (productSpec.PageSize) * ( (productSpec.PageIndex) - 1)    , productSpec.PageSize);

        }
        public ProductWithSpecsBrandAndCategory(int id) : base(p=>p.Id==id)
        {
            Includat.Add(p => p.ProductType);


        }
    }
}
