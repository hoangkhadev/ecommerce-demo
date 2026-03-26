using Ecommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.Category.Commands
{
    public record DeleteCategoryCommand(Guid Id) : IRequest;
    public class DeleteCategoryHandler(IApplicationDbContext context) : IRequestHandler<DeleteCategoryCommand>
    {
        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
                ??
                throw new KeyNotFoundException($"Category with ID '{request.Id}' not found");

            var hasProducts = await context.Products
                .AnyAsync(p => p.CategoryId == request.Id, cancellationToken);

            if (hasProducts) throw new InvalidOperationException("Category cannot be deleted because it contains products");

            context.Categories.Remove(category);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
