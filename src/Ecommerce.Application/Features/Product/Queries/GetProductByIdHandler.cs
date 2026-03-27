using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Features.Category.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.Product.Queries
{
    public record ProductDetailResponse(Guid Id, string Name, string Description, int Stock, decimal Price, CategoryResponse Category);
    public record GetProductByIdResponse(string Message, ProductDetailResponse Data);
    public record GetProductByIdQuery(Guid Id) : IRequest<GetProductByIdResponse>;

    public class GetProductByIdHandler(IApplicationDbContext context) : IRequestHandler<GetProductByIdQuery, GetProductByIdResponse>
    {
        public async Task<GetProductByIdResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await context.Products.Include(x => x.Category).FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Product with ID '{request.Id}' not found");

            var productResponse = new ProductDetailResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Stock,
                product.Price,
                new CategoryResponse(product.Category.Id, product.Category.Name)
            );
            return new GetProductByIdResponse("Get product success", productResponse);
        }
    }
}
