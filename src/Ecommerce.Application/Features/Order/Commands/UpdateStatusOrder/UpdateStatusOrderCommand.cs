
using MediatR;
using System.Text.Json.Serialization;

namespace Ecommerce.Application.Features.Order.Commands.UpdateStatusOrder;

public record UpdateStatusOrderCommand([property: JsonIgnore] Guid OrderId, string Status) : IRequest<Guid>;
