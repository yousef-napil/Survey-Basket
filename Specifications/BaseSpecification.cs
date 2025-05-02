using System.Linq.Expressions;

namespace Survey_Basket.Specifications;

public class BaseSpecification<T> : ISpecifications<T> where T : BaseEntity
{
    public Expression<Func<T, bool>> Criteria { get; set; }
    public Expression<Func<T, bool>> Any { get; set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
    public Expression<Func<T, object>> OrderBy { get; set; }
    public Expression<Func<T, object>> OrderByDescending { get; set; }
    public BaseSpecification()
    {
    }

    public void AddCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }
    public void AddAny(Expression<Func<T, bool>> any)
    {
        Any = any;
    }

    public void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }
    public void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }


}