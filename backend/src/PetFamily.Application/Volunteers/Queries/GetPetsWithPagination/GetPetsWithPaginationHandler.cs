
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;
using System.Linq.Expressions;

namespace PetFamily.Application.Volunteers.Queries.GetPetsWithPagination
{
    public class GetPetsWithPaginationHandler(
        IReadDbContext readDbContext) : IQueryHandler<PagedList<PetDto>, GetPetsWithPaginationQuery>
    {
        public async Task<Result<PagedList<PetDto>, ErrorList>> Handle(GetPetsWithPaginationQuery query,
            CancellationToken token = default)
        {
            var petsQuery = ApplyFilters(readDbContext.Pets, query);

            var keySelector = SortByProperty(query.SortBy);

            petsQuery = query.SortDirection?.ToLower() == "desc"
                ? petsQuery.OrderByDescending(keySelector)
                : petsQuery.OrderBy(keySelector);

            return await petsQuery.GetObjectsWithPagination(query.Page, query.PageSize, token);
        }

        private static IQueryable<PetDto> ApplyFilters(IQueryable<PetDto> petsQuery, GetPetsWithPaginationQuery query)
        {
            return petsQuery
                .WhereIf(!string.IsNullOrEmpty(query.Name), pet => pet.Name.Contains(query.Name!))
                .WhereIf(!string.IsNullOrEmpty(query.Color), pet => pet.Color.Contains(query.Color!))
                .WhereIf(query.MinAge.HasValue, pet => (DateTime.Now - pet.BirthDay.ToDateTime(TimeOnly.MinValue)).TotalDays / 365 >= query.MinAge)
                .WhereIf(query.MaxAge.HasValue, pet => (DateTime.Now - pet.BirthDay.ToDateTime(TimeOnly.MinValue)).TotalDays / 365 <= query.MaxAge)
                .WhereIf(query.SpeciesId.HasValue, pet => pet.SpeciesId == query.SpeciesId)
                .WhereIf(query.BreedId.HasValue, pet => pet.BreedId == query.BreedId)
                .WhereIf(!string.IsNullOrEmpty(query.Country), pet => pet.Address.Country == query.Country)
                .WhereIf(!string.IsNullOrEmpty(query.City), pet => pet.Address.City == query.City)
                .WhereIf(!string.IsNullOrEmpty(query.Street), pet => pet.Address.Street == query.Street)
                .WhereIf(query.PostalCode.HasValue, pet => pet.Address.PostalCode == query.PostalCode)
                .WhereIf(!string.IsNullOrEmpty(query.HouseNumber), pet => pet.Address.HouseNumber == query.HouseNumber)
                .WhereIf(!string.IsNullOrEmpty(query.FlatNumber), pet => pet.Address.FlatNumber == query.FlatNumber)
                .WhereIf(query.MinHeight.HasValue, pet => pet.Height >= query.MinHeight)
                .WhereIf(query.MaxHeight.HasValue, pet => pet.Height <= query.MaxHeight)
                .WhereIf(query.MinWeight.HasValue, pet => pet.Weight >= query.MinWeight)
                .WhereIf(query.MaxWeight.HasValue, pet => pet.Weight <= query.MaxWeight)
                .WhereIf(!string.IsNullOrEmpty(query.Phone), pet => pet.Phone == query.Phone)
                .WhereIf(query.VolunteerId.HasValue, pet => pet.VolunteerId == query.VolunteerId)
                .WhereIf(query.IsCastrated.HasValue, pet => pet.IsCastrated == query.IsCastrated)
                .WhereIf(query.IsVaccinated.HasValue, pet => pet.IsVaccinated == query.IsVaccinated)
                .WhereIf(query.HelpStatus.HasValue, pet => pet.Status == query.HelpStatus);
        }
        private Expression<Func<PetDto, object>> SortByProperty(string? sortBy)
        {
            if (string.IsNullOrEmpty(sortBy))
                return volunteer => volunteer.Id;

            Expression<Func<PetDto, object>> keySelector = sortBy?.ToLower() switch
            {
                "name" => prop => prop.Name,
                "country" => prop => prop.Address.Country,
                "city" => prop => prop.Address.City,
                "street" => prop => prop.Address.Street,
                "postalcode" => prop => prop.Address.PostalCode,
                "housenumber" => prop => prop.Address.HouseNumber,
                "flatnumber" => prop => prop.Address.FlatNumber,
                "birthday" => prop => prop.BirthDay,
                "breed" => prop => prop.BreedId,
                "species" => prop => prop.SpeciesId,
                "volunteer" => prop => prop.VolunteerId,
                "height" => prop => prop.Height,
                "weight" => prop => prop.Weight,
                _ => prop => prop.Name
            };
            return keySelector;
        }
    }
}
