using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models
{
    public class Volunteer : Shared.Entity<VolunteerId>
    {
        private readonly List<Pet> _pets = [];

        public FullName FullName { get; private set; } = null!;

        public Email Email { get; private set; } = default!;

        public Description Description { get; private set; } = default!;

        public int YearsExperience { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; } = default!;

        public SocialNetworkList? SocialNetworks { get; private set; } = null!;

        public RequisiteList? Requisites { get; private set; } = null!;

        public IReadOnlyList<Pet> Pets => _pets;

        public int PetsCountNeedHelp() => _pets.Count(p => p.HelpStatus == Enums.HelpStatus.NeedHelp);

        public int PetsCountLookingFoHome() => _pets.Count(p => p.HelpStatus == Enums.HelpStatus.LookingFoHome);

        public int PetsCountFoundHome() => _pets.Count(p => p.HelpStatus == Enums.HelpStatus.FoundHome);

        private Volunteer(VolunteerId id) : base(id) { }

        private Volunteer(VolunteerId id,
                          FullName fullName,
                          Email email,
                          Description description,
                          int yearsExperience,
                          PhoneNumber phoneNumber,
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
                                                      Email email,
                                                      Description description,
                                                      int yearsExperience,
                                                      PhoneNumber phoneNumber,
                                                      SocialNetworkList socialNetworks,
                                                      RequisiteList requisites)
        {
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
