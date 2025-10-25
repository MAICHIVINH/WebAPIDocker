namespace Application.DTOs
{

    public record CategoryDto(int Id, string Name, string Type, int? UserId, DateTime CreatedAt);

    public record CreateCategoryDto(string Name, string Type);
    public record UpdateCategoryDto(string Name, string Type);
}