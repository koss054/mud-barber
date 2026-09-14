using Microsoft.AspNetCore.Components;

namespace MudBarber.Web.Models;

public class FlowStepModel
{
    public string Title { get; set; } = null!;

    // Evaluated on every render so the Next button tracks the live model.
    public Func<bool> IsComplete { get; set; } = () => true;

    public RenderFragment RenderFragment { get; set; } = null!;
}
