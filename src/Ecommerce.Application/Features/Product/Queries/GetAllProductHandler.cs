using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Features.Category.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.Product.Queries
{
    public record ProductResponse(Guid Id, string Name, int Stock, decimal Price, CategoryResponse Category);
    public record PageResult<T>(List<T> Items, int TotalRecords, int PageNumber, int PageSize)
    {
        public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
    }
    public record GetAllProductResponse(string Message, PageResult<ProductResponse> Data);

    public record GetAllProductQuery(
        int PageNumber = 1,
        int PageSize = 10,
        Guid? CategoryId = null,
        decimal? MinPrice = null,
        decimal? MaxPrice = null
    ) : IRequest<GetAllProductResponse>;

    public class GetAllProductHandler(IApplicationDbContext context) : IRequestHandler<GetAllProductQuery, GetAllProductResponse>
    {
        public async Task<GetAllProductResponse> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var query = context.Products.AsNoTracking().AsQueryable();

            if (request.CategoryId.HasValue) query = query.Where(x => x.CategoryId == request.CategoryId);
            if (request.MinPrice.HasValue) query = query.Where(x => x.Price >= request.MinPrice);
            if (request.MaxPrice.HasValue) query = query.Where(x => x.Price <= request.MaxPrice);

            var totalRecords = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProductResponse(p.Id, p.Name, p.Stock, p.Price, new CategoryResponse(p.Category.Id, p.Category.Name))) // Map sang DTO
                .ToListAsync(cancellationToken);

            return new GetAllProductResponse("Get all product success", new PageResult<ProductResponse>(items, totalRecords, request.PageNumber, request.PageSize));
        }
    }
}
