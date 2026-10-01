using Microsoft.AspNetCore.Components;

namespace MudBarber.Web.Models;

public class StepModel
{
    public string Title { get; set; } = null!;

    public Func<string?> Summary { get; set; } = () => null;

    // Gates the Next button. Completion is only granted by clicking Next.
    public Func<bool> IsValid { get; set; } = () => true;

    public RenderFragment RenderFragment { get; set; } = null!;
}
