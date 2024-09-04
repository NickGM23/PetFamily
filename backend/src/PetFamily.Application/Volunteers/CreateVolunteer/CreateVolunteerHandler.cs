
using CSharpFunctionalExtensions;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteerHandler
    {
        private readonly IVolunteersRepository _repository;

        public CreateVolunteerHandler(IVolunteersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Guid, Error>>  Handle(CreateVolunteerRequest request, 
                                                       CancellationToken cancellationToken = default)
        {
            var volunteerId = VolunteerId.NewVolunteerId();

            var fullNameResult = FullName.Create(request.FullName.LastName,
                                                 request.FullName.FirstName,
                                                 request.FullName.Patronymic);
            if (fullNameResult.IsFailure)
                return fullNameResult.Error;

            SocialNetworkList socialNetworksList = new(request.SocialNetworksDTO.
                                                       Select(nw => SocialNetwork.Create(
                                                              nw.Name,
                                                              nw.Link).Value));

            RequisiteList requisitesList = new(request.RequisitesDTO.
                                               Select(r => Requisite.Create(
                                                      r.Name,
                                                      r.Description).Value));

            var volunteerResul = Volunteer.Create(volunteerId,
                                             fullNameResult.Value,
                                             request.Email,
                                             request.Description,
                                             request.YearsOfExperience,
                                             request.PhoneNumber,
                                             socialNetworksList,
                                             requisitesList);

            if (volunteerResul.IsFailure)
                return volunteerResul.Error;

            var volunteer = volunteerResul.Value;

            await _repository.Add(volunteer);

            return (Guid)volunteer.Id;
        }
    }
}
