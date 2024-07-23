﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public interface ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T,bool>> wherewor { set; get; }
        public List<Expression<Func<T, object>>> Includat { set; get; }

        public Expression<Func<T, object>> OrderBy { set; get; } 
        public Expression<Func<T, object>> OrderByDesc { set; get; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPagination { get; set; }

    }
}
