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
                          string phoneNumber,
                          SocialNetworkList socialNetworks,
                          RequisiteList requisites                          
                          ) : base(id) 
        {
            FullName = fullName;
            Email = email;
            Description = description;
            YearsExperience = yearsExperience;
            PhoneNumber = phoneNumber;
            SocialNetworks = socialNetworks;
            Requisites = requisites;
        }

        public static Result<Volunteer, Error> Create(VolunteerId id,
                                                      FullName  fullName,
                                                      string email,
                                                      string description,
                                                      int yearsExperience,
                                                      string phoneNumber,
                                                      SocialNetworkList socialNetworks,
                                                      RequisiteList requisites)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Errors.General.ValueIsInvalid("Email");
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                return Errors.General.ValueIsInvalid("Description");
            }
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return Errors.General.ValueIsInvalid("PhoneNumber");
            }

            var volunteer = new Volunteer(id,
                                          fullName,
                                          email,
                                          description,
                                          yearsExperience,
                                          phoneNumber,
                                          socialNetworks,
                                          requisites);
            return volunteer;
        }

    }
}
