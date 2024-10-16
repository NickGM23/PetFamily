
using Dapper;
using PetFamily.Core.Extensions;
using PetFamily.Core.Dtos;
using System.Data;
using System.Text;
using System.Text.Json;

namespace PetFamily.Core.Extensions
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

        public static async Task<List<VolunteerDto>> QueryVolunteersAsync(
            this IDbConnection connection, string sql, DynamicParameters parameters)
        {
            var result = await connection.QueryAsync<VolunteerDto, string, string, VolunteerDto>(
                sql,
                (volunteer, socialNetworksJson, requisitesJson) =>
                {
                    var requisites = JsonSerializer.Deserialize<RequisiteDto[]>(requisitesJson);
                    var socialNetworks = JsonSerializer.Deserialize<SocialNetworkDto[]>(socialNetworksJson);

                    volunteer.Requisites = requisites ?? [];
                    volunteer.SocialNetworks = socialNetworks ?? [];

                    return volunteer;
                },
                splitOn: "social_networks,requisites",
                param: parameters);
            return result.ToList();
        }
    }
}
