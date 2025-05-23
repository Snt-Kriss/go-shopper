using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Specifications
{
    public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
    {
        protected BaseSpecification() : this(null)
        {

        }
        public Expression<Func<T, bool>>? Criteria => criteria;

        public Expression<Func<T, object>>? OrderBy { get; set; }

        public Expression<Func<T, object>>? OrderByDescending { get; set; }

        public bool IsDistinct { get; private set; }

        public int Take { get; set; }

        public int Skip { get; set; }

        public bool IsPagingEnabled { get; set; }

        public IQueryable<T> ApplyCriteria(IQueryable<T> query)
        {
            if (criteria != null)
            {
                query = query.Where(Criteria);
            }
            return query;
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        protected void ApplyDistinct()
        {
            IsDistinct = true;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }

    public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria) :
     BaseSpecification<T>(criteria), ISpecification<T, TResult>
    {
         protected BaseSpecification() : this(null)
        {

        }
        public Expression<Func<T, TResult>>? Select { get; set; }

        protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
        {
            Select = selectExpression;
        }
    }
}