using System.Linq.Expressions;

namespace Survey_Basket.Specifications;

public interface ISpecifications<T> where T : BaseEntity
{
    public Expression<Func<T, bool>> Criteria { get; set; }
    public Expression<Func<T, bool>> Any { get; set; }
    public Expression<Func<T, bool>> Except { get; set; }
    public List<Expression<Func<T, object>>> Includes { get; }
    public Expression<Func<T, object>> OrderBy { get; set; }
    public Expression<Func<T, object>> OrderByDescending { get; set; }



}
