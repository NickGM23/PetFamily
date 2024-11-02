﻿

using PetFamily.Accounts.Application.Commands.Register;

namespace PetFamily.Accounts.Presentation.Requests
{
    public record RegisterUserRequest(
        string? Name,
        string? Surname,
        string? Patronymic, 
        string UserName, 
        string Email, 
        string Password)
    {
        public RegisterUserCommand ToCommand() =>
             new(Name, Surname, Patronymic, UserName, Email, Password);

    }
}
