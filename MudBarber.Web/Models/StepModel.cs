using Microsoft.AspNetCore.Components;

namespace MudBarber.Web.Models;

public class StepModel
{
    public string Title { get; set; } = null!;

    public Func<string?> Summary { get; set; } = () => null;

    // Evaluated on every render so the Next button tracks the live model.
    public Func<bool> IsCompleted { get; set; } = () => true;

    public RenderFragment RenderFragment { get; set; } = null!;
}
