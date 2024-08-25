using CSharpFunctionalExtensions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Base;
using PetFamily.Domain.Models.Shared;

namespace PetFamily.Domain.Models
{
    public class Pet : BaseModel
    {
        private readonly List<Requisite> _requisites = new List<Requisite>();

        public string Name { get; private set; } = string.Empty;

        public string Species { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public string Breed { get; private set; } = string.Empty;

        public string Color { get; private set; } = string.Empty;

        public string HealthInfo { get; private set; } = string.Empty;

        public string Address { get; private set; } = string.Empty;

        public double Weight { get; private set; }

        public double Height { get; private set; }

        public string PhoneNumber { get; private set; } = string.Empty;

        public bool IsCastrated { get; private set; } 

        public DateOnly BirthDay { get; private set; }

        public bool IsVaccinated { get; private set; }

        public HelpStatus HelpStatus { get; private set; }

        public IReadOnlyList<Requisite> Requisites => _requisites;

        public void AddRequisite(Requisite requisite)
        {
            _requisites.Add(requisite);
        }

        public DateTime DateCteate { get; private set; }

        private Pet(Guid id) : base(id) { }

        private Pet(Guid id,
                    string name, 
                    string species, 
                    string description, 
                    string breed,
                    string color, 
                    string healthInfo, 
                    string address, 
                    double weight,
                    double height,
                    string phoneNumber,
                    bool isCastrated,
                    DateOnly birthDay, 
                    bool isVaccinated,
                    HelpStatus helpStatus) : base(id)
        {
            Name = name;
            Species = species;
            Description = description;
            Breed = breed;
            Color = color;
            HealthInfo = healthInfo;
            Address = address;
            Weight = weight;
            Height = height;
            PhoneNumber = phoneNumber;
            IsCastrated = isCastrated;
            BirthDay = birthDay;
            IsVaccinated = isVaccinated;
            HelpStatus = helpStatus;
            DateCteate = DateTime.UtcNow;
        }

        public static Result<Pet> Create(Guid id, 
                                         string name, 
                                         string species, 
                                         string description, 
                                         string breed,
                                         string color, 
                                         string healthInfo, 
                                         string address,
                                         double weight,
                                         double height, 
                                         string phoneNumber, 
                                         bool isCastrated, 
                                         DateOnly birthDay,
                                         bool isVaccinated, 
                                         HelpStatus helpStatus)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<Pet>("Name can not be empty");
            }
            if (string.IsNullOrWhiteSpace(species))
            {
                return Result.Failure<Pet>("Species can not be empty");
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                return Result.Failure<Pet>("Description can not be empty");
            }
            if (string.IsNullOrWhiteSpace(breed))
            {
                return Result.Failure<Pet>("Breed can not be empty");
            }
            if (string.IsNullOrWhiteSpace(color))
            {
                return Result.Failure<Pet>("Color can not be empty");
            }
            if (string.IsNullOrWhiteSpace(healthInfo))
            {
                return Result.Failure<Pet>("HealthInfo can not be empty");
            }
            if (string.IsNullOrWhiteSpace(address))
            {
                return Result.Failure<Pet>("Address can not be empty");
            }
            if (weight <= 0)
            {
                return Result.Failure<Pet>("Incorrect value for field Weight");
            }
            if (height <= 0)
            {
                return Result.Failure<Pet>("Incorrect value for field Height");
            }
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return Result.Failure<Pet>("PhoneNumber can not be empty");
            }

            var pet = new Pet(id,
                              name,
                              species,
                              description,
                              breed,                      
                              color, 
                              healthInfo,
                              address,
                              weight,
                              height,
                              phoneNumber,
                              isCastrated, 
                              birthDay,
                              isVaccinated, 
                              helpStatus);
            return pet;
        }

    }
}
