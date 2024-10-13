
using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Extensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Dtos;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;
using System.Text;

namespace PetFamily.SpeciesManagement.Application.Queries.GetSpeciesWithPanination
{
    public class GetSpeciesWithPaginationHandler : IQueryHandler<PagedList<SpeciesDto>, GetSpeciesWithPaginationQuery>
    {
        private readonly IValidator<GetSpeciesWithPaginationQuery> _validator;
        private readonly ILogger<GetSpeciesWithPaginationHandler> _logger;
        private readonly ISqlConnectionFactory _factory;

        public GetSpeciesWithPaginationHandler(
            IValidator<GetSpeciesWithPaginationQuery> validator,
            ILogger<GetSpeciesWithPaginationHandler> logger,
            ISqlConnectionFactory factory)
        {
            _validator = validator;
            _logger = logger;
            _factory = factory;
        }

        public async Task<Result<PagedList<SpeciesDto>, ErrorList>> Handle(
            GetSpeciesWithPaginationQuery query,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ToList();

            var connection = _factory.Create();

            var parameters = new DynamicParameters();

            var totalCount = await connection.ExecuteScalarAsync<long>(
                "  SELECT COUNT(1) FROM species;");

            var sql = new StringBuilder("""
                                    SELECT
                                        id, 
                                        name, 
                                        description 
                                    FROM 
                                        species 
                                    """);

            sql.ApplyPagination(parameters, query.Page, query.PageSize);

            var species = await connection.QueryAsync<SpeciesDto>(sql.ToString(), parameters);

            var pagedList = new PagedList<SpeciesDto>()
            {
                Items = species.ToList(),
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount
            };

            return pagedList;
        }
    }
}

