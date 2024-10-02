﻿using PetFamily.Application.Volunteers.UpdatePetStatus;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record UpdatePetStatusRequest
    {
        public UpdatePetStatusRequest(string status)
        {
            Status = status;
        }

        /// <summary>
        /// Status of the Pet. It can be "LookingFoHome", "FoundHome"
        /// </summary>
        /// <example>FoundHome</example>
        public string Status { get; }

        public UpdatePetStatusCommand ToCommand(Guid volunteerId, Guid petId) =>
            new(volunteerId, petId, Status);
    }
}
