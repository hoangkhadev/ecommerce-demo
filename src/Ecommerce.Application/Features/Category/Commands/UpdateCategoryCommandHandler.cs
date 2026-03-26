using Ecommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Ecommerce.Application.Features.Category.Commands
{
    public record UpdateCategoryResponse(string Message, Guid Id);
    public record UpdateCategoryCommand([property: JsonIgnore] Guid Id, string Name) : IRequest<UpdateCategoryResponse>;
    public class UpdateCategoryCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResponse>
    {
        public async Task<UpdateCategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Category with ID '{request.Id}' not found");

            var existingName = await context.Categories.AnyAsync(c => c.Name == request.Name && c.Id != request.Id, cancellationToken);
            if (existingName)
            {
                var failures = new List<FluentValidation.Results.ValidationFailure>
                {
                    new("Name", "Category name is already exists")
                };
                throw new FluentValidation.ValidationException(failures);
            }

            category.Name = request.Name;
            await context.SaveChangesAsync(cancellationToken);

            return new UpdateCategoryResponse("Update category success", category.Id);
        }
    }
}
