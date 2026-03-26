using Ecommerce.Application.Common.Interfaces;
using CategoryEntity = Ecommerce.Domain.Entities.Category;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.Category.Commands
{
    public record CreateCategoryResponse(string Message, Guid CategoryId);
    public record CreateCategoryCommand(string Name) : IRequest<CreateCategoryResponse>;

    public class CreateCategoryCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
    {
        public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var exists = await context.Categories.AnyAsync(c => c.Name == request.Name, cancellationToken);
            if (exists)
            {
                var failures = new List<FluentValidation.Results.ValidationFailure>
                {
                    new("Name", "Category name is already exists")
                };
                throw new FluentValidation.ValidationException(failures);
            }

            var newCategory = new CategoryEntity
            {
                Name = request.Name,
            };
            context.Categories.Add(newCategory);
            await context.SaveChangesAsync(cancellationToken);

            return new CreateCategoryResponse("Create category success", newCategory.Id);
        }
    }
}
