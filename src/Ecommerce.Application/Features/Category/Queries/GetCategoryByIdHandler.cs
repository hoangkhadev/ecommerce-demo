using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.Category.Queries
{
    public record CategoryDetailsResponse(Guid Id, string Name);
    public record GetCategoryByIdResponse(string Message, CategoryDetailsResponse Data);
    public record GetCategoryByIdQuery(Guid Id) : IRequest<GetCategoryByIdResponse>;

    public class GetCategoryByIdHandler(IApplicationDbContext context) : IRequestHandler<GetCategoryByIdQuery, GetCategoryByIdResponse>
    {
        public async Task<GetCategoryByIdResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await context.Categories
                .AsNoTracking()
                .Where(c => c.Id == request.Id)
                .Select(c => new CategoryDetailsResponse(c.Id, c.Name))
                .FirstOrDefaultAsync(cancellationToken)
                ??
                throw new KeyNotFoundException($"Category with ID '{request.Id}' not found");

            return new GetCategoryByIdResponse("Get category detail success", category);
        }
    }
}
