using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specification.ProductSpecs
{
    public class ProductSpecParams
    {
        public int pageIndex=1;
        private const int MaxPageSize=5;

        private int pageSize=MaxPageSize;
        public int PageSize {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public int PageIndex {  get; set; }
        public string? Sort { get; set; }
        public int? CategoryId { get; set; }
        private string? search;
        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
      




    }
}
