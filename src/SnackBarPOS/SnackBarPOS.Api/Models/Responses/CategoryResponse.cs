namespace SnackBarPOS.Api.Models.Responses;

public record CategoryResponse(
    int Id,
    string Name,
    string? Description,
    string? IconEmoji,
    int SortOrder,
    bool IsActive,
    int ProductCount
);
