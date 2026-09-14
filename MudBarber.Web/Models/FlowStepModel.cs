using Microsoft.AspNetCore.Components;

namespace MudBarber.Web.Models;

public class FlowStepModel
{
    public string Title { get; set; } = null!;

    public RenderFragment RenderFragment { get; set; } = null!;
}