
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks
{
    public class UpdateSocialNetworksHandler
    {
        private readonly IVolunteersRepository _repository;
        private readonly IValidator<UpdateSocialNetworksCommand> _validator;
        private readonly ILogger<UpdateSocialNetworksHandler> _logger;

        public UpdateSocialNetworksHandler(
            IVolunteersRepository repository,
            IValidator<UpdateSocialNetworksCommand> validator,
            ILogger<UpdateSocialNetworksHandler> logger)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateSocialNetworksCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToList();

            var volunteerResult = await _repository.GetById(command.VolunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            List<SocialNetwork> socialNetworks = [];
            foreach (var socialNetwork in command.SocialNetworks)
            {
                socialNetworks.Add(SocialNetwork.Create(socialNetwork.Name, socialNetwork.Link).Value);
            }

            var socialNetworksToUpdate = new SocialNetworkList(socialNetworks);

            volunteerResult.Value.UpdateSocialNetworks(socialNetworksToUpdate);

            await _repository.Save(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Social medias of volunteer with {id} has been updated", command.VolunteerId);

            return command.VolunteerId;
        }
    }
}
