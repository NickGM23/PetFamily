using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Base;
using PetFamily.Domain.Models.Shared;

namespace PetFamily.Domain.Models
{
    public class Volunteer : BaseModel
    {
        private readonly List<SocialNetwork> _socialNetworks = new List<SocialNetwork>();

        private readonly List<Requisite> _requisites = new List<Requisite>();

        private readonly List<Pet> _pets = new List<Pet>();

        public string Name { get; private set; } = string.Empty;

        public string FIO { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public int YearsExperience { get; private set; }

        public string PhoneNumber { get; private set; } = string.Empty;

        public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;

        public void AddSocialNetwork(SocialNetwork socialNetwork)
        {
            _socialNetworks.Add(socialNetwork);
        }

        public IReadOnlyList<Requisite> Requisites => _requisites;

        public void AddRequisite(Requisite requisite)
        {
            _requisites.Add(requisite);
        }

        public IReadOnlyList<Pet> Pets => _pets;

        public void AddPet(Pet pet)
        {
            _pets.Add(pet);
        }

        public int PetsCountNeedHelp() => _pets.Count(p => p.HelpStatus == Enums.HelpStatus.NeedHelp);

        public int PetsCountLookingFoHome() => _pets.Count(p => p.HelpStatus == Enums.HelpStatus.LookingFoHome);

        public int PetsCountFoundHome() => _pets.Count(p => p.HelpStatus == Enums.HelpStatus.FoundHome);

        private Volunteer(Guid id) : base(id) { }

        private Volunteer(Guid id,
                          string name,
                          string fio,
                          string email,
                          string description,
                          int yearsExperience,
                          string phoneNumber
                          ) : base(id) 
        {   
            Name = name;    
            FIO = fio;
            Email = email;
            Description = description;
            YearsExperience = yearsExperience;
            PhoneNumber = phoneNumber;
        }

        public static Result<Volunteer> Create(Guid id,
                                               string name,
                                               string fio,
                                               string email,
                                               string description,
                                               int yearsExperience,
                                               string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<Volunteer>("Name can not be empty");
            }
            if (string.IsNullOrWhiteSpace(fio))
            {
                return Result.Failure<Volunteer>("FIO can not be empty");
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
                                          name,
                                          fio,
                                          email,
                                          description,
                                          yearsExperience,
                                          phoneNumber);
            return volunteer;
        }

    }
}
