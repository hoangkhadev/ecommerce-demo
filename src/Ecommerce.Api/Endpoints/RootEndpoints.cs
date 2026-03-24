using Ecommerce.Api.Models;

namespace Ecommerce.Api.Endpoints
{
    public static class RootEndpoints
    {
        public static void MapRootEndpoints(this WebApplication app)
        {
            app.MapGet("/", (HttpContext http) =>
            {
                return new ApiRootResponse(
                    "Welcome to the E-commerce API!",
                    new ApiEndpoints(
                        $"{http.Request.Scheme}://{http.Request.Host}/health",
                        $"{http.Request.Scheme}://{http.Request.Host}/api/v1",
                        $"{http.Request.Scheme}://{http.Request.Host}/swagger/index.html"
                    )
                );
            }).WithName("Root");
        }
    }
}
