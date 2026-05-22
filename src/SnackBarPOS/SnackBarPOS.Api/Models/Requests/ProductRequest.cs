namespace SnackBarPOS.Api.Models.Requests;

public record CreateProductRequest(
    string Name,
    string? Description,
    decimal Price,
    int CategoryId,
    string? ImageUrl
);

public record UpdateProductRequest(
    string Name,
    string? Description,
    decimal Price,
    int CategoryId,
    string? ImageUrl,
    bool IsActive,
    bool IsAvailable
);
