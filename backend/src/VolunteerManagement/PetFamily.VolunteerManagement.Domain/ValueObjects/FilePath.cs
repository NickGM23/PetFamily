﻿
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Domain.ValueObjects
{
    public record FilePath
    {
        private FilePath(string path)
        {
            Path = path;
        }

        public string Path { get; }

        public static Result<FilePath, Error> Create(Guid path, string extension)
        {
            var fullPath = path + extension;

            return new FilePath(fullPath);
        }

        public static Result<FilePath, Error> Create(string fullPath)
        {
            return new FilePath(fullPath);
        }
    }
}
