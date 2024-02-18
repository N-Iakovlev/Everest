﻿using Microsoft.AspNetCore.Http;

namespace Everest.Domain;

#region << Using >>

using FluentValidation;
using Incoding.Core.CQRS.Core;

#endregion

public class DeleteProductCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        Repository.Delete(Repository.GetById<Product>(Id));
    }
}

public class AddOrEditProductCommand : CommandBase
{
    public int? Id { get; set; }
    public string ProductName { get; set; }
   
    public IFormFile ProductPhoto { get; set; }

    protected override void Execute()
    {
        var isNew = Id.GetValueOrDefault() == 0;
        Product pr = isNew ? new Product() : Repository.GetById<Product>(Id.GetValueOrDefault());
        pr.ProductName = ProductName;
        if (ProductPhoto != null && ProductPhoto.Length > 0)
        {

            using (var memoryStream = new MemoryStream())
            {
                ProductPhoto.CopyTo(memoryStream);
                pr.ProductPhoto = memoryStream.ToArray(); 
            }
        }
        Repository.SaveOrUpdate(pr);
    }

    public class Validator : AbstractValidator<AddOrEditProductCommand>
    {
        public Validator()
        {
            RuleFor(pr => pr.ProductName).NotEmpty();

            
        }
    }

    public class AsQuery : QueryBase<AddOrEditProductCommand>
    {
        public int Id { get; set; }

        protected override AddOrEditProductCommand ExecuteResult()
        {
            var pr = Repository.GetById<Product>(Id);
            if (pr == null)
                return new AddOrEditProductCommand();

            return new AddOrEditProductCommand()
                   {
                           Id = pr.Id,
                           ProductName = pr.ProductName,
                          
                   };
        }
    }
}