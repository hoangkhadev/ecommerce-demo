using Ecommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductEntity = Ecommerce.Domain.Entities.Product;

namespace Ecommerce.Application.Features.Product.Commands
{
    public record CreateProductResponse(string Message, Guid Id);
    public record CreateProductCommand(string Name, string Description, decimal Price, Guid CategoryId, int Stock) : IRequest<CreateProductResponse>;
    public class CreateProductHandler(IApplicationDbContext context) : IRequestHandler<CreateProductCommand, CreateProductResponse>
    {
        public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var categoryExists = await context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
            if (!categoryExists)
            {
                var failuers = new List<FluentValidation.Results.ValidationFailure>
                {
                    new("CategoryId", "The specified category does not exist")
                };
                throw new FluentValidation.ValidationException(failuers);
            }

            var newProduct = new ProductEntity
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                CategoryId = request.CategoryId
            };

            context.Products.Add(newProduct);
            await context.SaveChangesAsync(cancellationToken);

            return new CreateProductResponse("Create product success", newProduct.Id);
        }
    }
}
