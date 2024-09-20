
using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.AddPet;

namespace PetFamily.API.Controllers.Volonteers.Requests
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
            IEnumerable<RequisiteDto> requisites)
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
        }

        public string Name { get; }
        
        public string Description { get; }
        
        public string Color { get; }
        
        public string HealthInfo { get; }
        
        public AddressDto Address { get; }
        
        public int Weight { get; }
        
        public int Height { get; }
        
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
                Requisites);
    }
}