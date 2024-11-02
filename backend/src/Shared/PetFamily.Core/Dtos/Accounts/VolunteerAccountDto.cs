﻿
namespace PetFamily.Core.Dtos.Accounts
{
    public class VolunteerAccountDto
    {
        public Guid Id { get; init; }
        public int Experience { get; init; }
        public IEnumerable<RequisiteDto> Requisites { get; init; } = [];
        public Guid UserId { get; init; }
    }
}
