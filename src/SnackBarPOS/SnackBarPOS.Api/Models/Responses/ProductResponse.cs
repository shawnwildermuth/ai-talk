namespace SnackBarPOS.Api.Models.Responses;

public record ProductResponse(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    int CategoryId,
    string CategoryName,
    string? CategoryEmoji,
    string? ImageUrl,
    bool IsActive,
    bool IsAvailable
);
