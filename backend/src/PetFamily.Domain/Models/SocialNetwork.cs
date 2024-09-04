﻿
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models
{
    public record SocialNetwork
    {
        public string Name { get; } = string.Empty;

        public string Link { get; } = string.Empty;

        private SocialNetwork(string name, string link)
        {
            Name = name;
            Link = link;
        }

        public static Result<SocialNetwork, Error> Create(string name, string link)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Errors.General.ValueIsInvalid("Name");
            }
            if (string.IsNullOrWhiteSpace(link))
            {
                return Errors.General.ValueIsInvalid("Link");
            }

            return new SocialNetwork(name, link);
        }
    }
}
