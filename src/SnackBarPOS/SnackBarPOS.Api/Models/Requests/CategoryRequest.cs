namespace SnackBarPOS.Api.Models.Requests;

public record CreateCategoryRequest(
    string Name,
    string? Description,
    string? IconEmoji,
    int SortOrder
);

public record UpdateCategoryRequest(
    string Name,
    string? Description,
    string? IconEmoji,
    int SortOrder,
    bool IsActive
);
