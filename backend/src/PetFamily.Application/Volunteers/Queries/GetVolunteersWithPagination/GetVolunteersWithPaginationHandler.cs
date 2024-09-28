using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;
using System.Text;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination
{
    public class GetVolunteersWithPaginationHandler
        : IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>
    {
        private readonly ILogger<GetVolunteersWithPaginationHandler> _logger;
        private readonly ISqlConnectionFactory _factory;

        public GetVolunteersWithPaginationHandler(
            ILogger<GetVolunteersWithPaginationHandler> logger,
            ISqlConnectionFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        public async Task<Result<PagedList<VolunteerDto>, ErrorList>> Handle(
            GetVolunteersWithPaginationQuery query,
            CancellationToken cancellationToken)
        {
            var connection = _factory.Create();

            var parameters = new DynamicParameters();

            var totalCount = await connection.ExecuteScalarAsync<long>(
                "  SELECT COUNT(1) FROM volunteers;");

            var sql = new StringBuilder("""
                                    SELECT 
                                        id, 
                                        first_name, 
                                        last_name,
                                        patronymic, 
                                        description, 
                                        email, 
                                        years_experience,
                                        phone_number, 
                                        social_networks,  
                                        requisites  
                                    FROM 
                                        volunteers 
                                    """);

            sql.ApplyPagination(parameters, query.Page, query.PageSize);

            var volunteers = await connection.QueryVolunteersAsync(sql.ToString(), parameters);

            var pagedList = new PagedList<VolunteerDto>()
            {
                Items = volunteers.ToList(),
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount
            };

            return pagedList;
        }
    }
}
