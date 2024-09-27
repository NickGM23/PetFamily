
using Dapper;
using System.Text;

namespace PetFamily.Application.Extensions
{
    public static class SqlExtensions
    {
        public static void ApplyPagination(
            this StringBuilder sqlBuilder,
            DynamicParameters parameters,
            int page,
            int pageSize)
        {
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@Offset", (page - 1) * pageSize);

            sqlBuilder.Append(" LIMIT @PageSize OFFSET @Offset");
        }
    }
}
