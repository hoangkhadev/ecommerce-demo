using Ecommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.Category.Queries
{
    public record CategoryResponse(Guid Id, string Name);
    public record GetAllCategoryResponse(string Message, IEnumerable<CategoryResponse> Data);
    public record GetAllCategoriesQuery : IRequest<GetAllCategoryResponse>;

    public class GetAllCategoriesHandler(IApplicationDbContext context) : IRequestHandler<GetAllCategoriesQuery, GetAllCategoryResponse>
    {
        public async Task<GetAllCategoryResponse> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await context.Categories.AsNoTracking().Select(c => new CategoryResponse(c.Id, c.Name)).ToListAsync(cancellationToken);
            return new GetAllCategoryResponse("Get all category success", categories);
        }
    }
}
