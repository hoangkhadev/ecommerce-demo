using Ecommerce.Api.Endpoints.Coupon;
using Ecommerce.Api.Endpoints.Review;

namespace Ecommerce.Api.Endpoints
{
    public static class EndpointExtensions
    {
        public static void MapEndpointDefinitions(this IEndpointRouteBuilder app)
        {
            var endpointDefinitions = typeof(IEndpointDefinition).Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IEndpointDefinition))
                        && !t.IsInterface
                        && !t.IsAbstract);

            foreach (var type in endpointDefinitions)
            {
                var instance = Activator.CreateInstance(type) as IEndpointDefinition;
                instance?.DefineEndpoints(app);
            }

            app.MapCouponEndpoints();
            app.MapReviewEndpoints();
        }
    }
}
