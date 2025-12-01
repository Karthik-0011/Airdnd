using System.Linq.Expressions;

namespace Airdnd.Models.DataLayer.Repositories
{
    public class QueryOptions<T>
    {
        public Expression<Func<T, bool>> Where { get; set; } = null!;
        public Expression<Func<T, object>> OrderBy { get; set; } = null!;
        public string Includes { get; set; } = string.Empty;

        public bool HasWhere => Where != null;
        public bool HasOrderBy => OrderBy != null;
    }
}