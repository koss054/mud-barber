using Microsoft.AspNetCore.Components;

namespace MudBarber.Web.Models;

public class StepModel
{
    public string Title { get; set; } = null!;

    public Func<string?> Summary { get; set; } = () => null;

    // Displayed in the previous step's "Next" button.
    public string? EntryLabel { get; set; }

    // Gates the Next button. Completion is only granted by clicking Next.
    public Func<bool> IsValid { get; set; } = () => true;

    public RenderFragment RenderFragment { get; set; } = null!;
}
