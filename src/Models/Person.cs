namespace DIPS_lab01.Models;

using Swashbuckle.AspNetCore.Annotations;

public partial class Person
{
    [SwaggerSchema(ReadOnly = true)]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? Age { get; set; }

    public string? Address { get; set; }

    public string? Work { get; set; }
}
