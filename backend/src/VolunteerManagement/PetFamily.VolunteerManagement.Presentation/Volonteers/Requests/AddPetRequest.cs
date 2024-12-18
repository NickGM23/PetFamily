﻿using PetFamily.Core.Dtos;
using PetFamily.VolunteerManagement.Application.Commands.AddPet;

namespace PetFamily.VolunteerManagement.Presentation.Volonteers.Requests
{
    /// <summary>
    /// Add Pet Request
    /// </summary>
    public record AddPetRequest
    {
        public AddPetRequest(string name,
            string description,
            string color,
            string healthInfo,
            AddressDto address,
            int weight,
            int height,
            string phoneNumber,
            bool isCastrated,
            DateOnly birthDate,
            bool isVaccinated,
            string status,
            IEnumerable<RequisiteDto> requisites,
            Guid speciesId,
            Guid breedId)
        {
            Name = name;
            Description = description;
            Color = color;
            HealthInfo = healthInfo;
            Address = address;
            Weight = weight;
            Height = height;
            PhoneNumber = phoneNumber;
            IsCastrated = isCastrated;
            BirthDate = birthDate;
            IsVaccinated = isVaccinated;
            Status = status;
            Requisites = requisites;
            SpeciesId = speciesId;
            BreedId = breedId;
        }

        public string Name { get; }

        public string Description { get; }

        public string Color { get; }

        public string HealthInfo { get; }

        public AddressDto Address { get; }

        public int Weight { get; }

        public int Height { get; }

        /// <example>+380967875645</example>
        public string PhoneNumber { get; }

        public bool IsCastrated { get; }

        public DateOnly BirthDate { get; }

        public bool IsVaccinated { get; }

        /// <summary>
        /// Status of the Pet. It can be "NeedHelp", "LookingFoHome", "FoundHome"
        /// </summary>
        /// <example>NeedHelp</example>
        public string Status { get; }

        public IEnumerable<RequisiteDto> Requisites { get; }

        public Guid SpeciesId { get; }

        public Guid BreedId { get; }

        public AddPetCommand ToCommand(Guid volunteerId) =>
            new(volunteerId,
                Name,
                Description,
                Color,
                HealthInfo,
                Address,
                Weight,
                Height,
                PhoneNumber,
                IsCastrated,
                BirthDate,
                IsVaccinated,
                Status,
                Requisites,
                SpeciesId,
                BreedId);
    }
}