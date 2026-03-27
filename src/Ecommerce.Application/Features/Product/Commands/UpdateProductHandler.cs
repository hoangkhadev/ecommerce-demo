using Ecommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Ecommerce.Application.Features.Product.Commands
{
    public record UpdateProductRequest(string Name, string Description, decimal Price, int? Stock, Guid CategoryId);
    public record UpdateProductResponse(string Message, Guid Id);
    public record UpdateProductCommand([property: JsonIgnore] Guid Id, UpdateProductRequest Data) : IRequest<UpdateProductResponse>;

    public class UpdateProductHandler(IApplicationDbContext context) : IRequestHandler<UpdateProductCommand, UpdateProductResponse>
    {
        public async Task<UpdateProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Product with ID '{request.Id}' not found");

            var categoryExists = await context.Categories.AnyAsync(c => c.Id == request.Data.CategoryId, cancellationToken);
            if (!categoryExists)
            {
                var failures = new List<FluentValidation.Results.ValidationFailure>
                {
                    new("CategoryId", "Category Id not found")
                };
                throw new FluentValidation.ValidationException(failures);
            }

            product.Name = request.Data.Name;
            product.Description = request.Data.Description;
            product.Price = request.Data.Price;
            product.Stock = request.Data.Stock ?? product.Stock;
            product.CategoryId = request.Data.CategoryId;

            await context.SaveChangesAsync(cancellationToken);

            return new UpdateProductResponse("Update product success", product.Id);
        }
    }
}
