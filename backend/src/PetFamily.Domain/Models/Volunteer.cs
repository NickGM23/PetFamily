using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models
{
    public class Volunteer : Shared.Entity<VolunteerId>
    {
        private readonly List<Pet> _pets = new List<Pet>();

        public FullName FullName { get; private set; } = null!;

        public string Email { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public int YearsExperience { get; private set; }

        public string PhoneNumber { get; private set; } = string.Empty;

        public SocialNetworkList? SocialNetworks { get; private set; } = null!;

        public RequisiteList? Requisites { get; private set; } = null!;

        public IReadOnlyList<Pet> Pets => _pets;

        public void AddPet(Pet pet)
        {
            _pets.Add(pet);
        }

        public int PetsCountNeedHelp() => _pets.Count(p => p.HelpStatus == Enums.HelpStatus.NeedHelp);

        public int PetsCountLookingFoHome() => _pets.Count(p => p.HelpStatus == Enums.HelpStatus.LookingFoHome);

        public int PetsCountFoundHome() => _pets.Count(p => p.HelpStatus == Enums.HelpStatus.FoundHome);

        private Volunteer(VolunteerId id) : base(id) { }

        private Volunteer(VolunteerId id,
                          FullName fullName,
                          string email,
                          string description,
                          int yearsExperience,
                          string phoneNumber
                          ) : base(id) 
        {
            FullName = fullName;
            Email = email;
            Description = description;
            YearsExperience = yearsExperience;
            PhoneNumber = phoneNumber;
        }

        public static Result<Volunteer> Create(VolunteerId id,
                                               FullName  fullName,
                                               string email,
                                               string description,
                                               int yearsExperience,
                                               string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(fullName.FirstName))
            {
                return Result.Failure<Volunteer>("First name can not be empty");
            }
            if (string.IsNullOrWhiteSpace(fullName.LastName))
            {
                return Result.Failure<Volunteer>("Last name can not be empty");
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                return Result.Failure<Volunteer>("Email can not be empty");
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                return Result.Failure<Volunteer>("Description can not be empty");
            }
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return Result.Failure<Volunteer>("PhoneNumber can not be empty");
            }

            var volunteer = new Volunteer(id,
                                          fullName,
                                          email,
                                          description,
                                          yearsExperience,
                                          phoneNumber);
            return volunteer;
        }

    }
}
