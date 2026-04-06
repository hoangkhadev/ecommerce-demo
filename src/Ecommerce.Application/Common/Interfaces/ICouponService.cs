namespace Ecommerce.Application.Common.Interfaces
{
    public interface ICouponService
    {
        Task<string> GenerateUniqueCode(string prefix = "CP", int length = 6, CancellationToken cancellationToken = default);
    }
}
