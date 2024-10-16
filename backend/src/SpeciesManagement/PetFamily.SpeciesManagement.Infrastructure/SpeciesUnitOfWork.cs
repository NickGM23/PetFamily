﻿using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.SpeciesManagement.Application;
using System.Data;

namespace PetFamily.SpeciesManagement.Infrastructure
{
    public class SpeciesUnitOfWork(SpeciesWriteDbContext context) : ISpeciesUnitOfWork
    {
        public async Task<IDbTransaction> BeginTransaction(CancellationToken token = default)
        {
            var transaction = await context.Database.BeginTransactionAsync(token);
            return transaction.GetDbTransaction();
        }

        public async Task SaveChanges(CancellationToken token = default) =>
            await context.SaveChangesAsync(token);
    }
}
