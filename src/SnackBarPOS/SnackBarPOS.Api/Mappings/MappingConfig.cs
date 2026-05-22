using Mapster;
using SnackBarPOS.Api.Entities;
using SnackBarPOS.Api.Models.Requests;
using SnackBarPOS.Api.Models.Responses;

namespace SnackBarPOS.Api.Mappings;

public static class MappingConfig
{
    public static void Configure()
    {
        // Category mappings
        TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
            .Map(dest => dest.ProductCount, src => src.Products.Count(p => p.IsActive));

        TypeAdapterConfig<CreateCategoryRequest, Category>.NewConfig()
            .Map(dest => dest.IsActive, _ => true)
            .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow);

        TypeAdapterConfig<UpdateCategoryRequest, Category>.NewConfig();

        // Product mappings
        TypeAdapterConfig<Product, ProductResponse>.NewConfig()
            .Map(dest => dest.CategoryName, src => src.Category.Name)
            .Map(dest => dest.CategoryEmoji, src => src.Category.IconEmoji);

        TypeAdapterConfig<CreateProductRequest, Product>.NewConfig()
            .Map(dest => dest.IsActive, _ => true)
            .Map(dest => dest.IsAvailable, _ => true)
            .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
            .Map(dest => dest.UpdatedAt, _ => DateTime.UtcNow);

        TypeAdapterConfig<UpdateProductRequest, Product>.NewConfig()
            .Map(dest => dest.UpdatedAt, _ => DateTime.UtcNow);

        // Order mappings
        TypeAdapterConfig<OrderItem, OrderItemResponse>.NewConfig()
            .Map(dest => dest.LineTotal, src => src.UnitPrice * src.Quantity);

        TypeAdapterConfig<Order, OrderResponse>.NewConfig()
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.PaymentMethod, src => src.PaymentMethod.HasValue ? src.PaymentMethod.ToString() : null)
            .Map(dest => dest.Items, src => src.Items.Adapt<IEnumerable<OrderItemResponse>>());
    }
}
