using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Kirel.Blazor.Entities;

/// <summary>
/// MudBlazor Dialog for viewing and editing entity
/// </summary>
/// <typeparam name="TCreateDto">Create entity data transfer object</typeparam>
/// <typeparam name="TUpdateDto">Update entity data transfer object</typeparam>
/// <typeparam name="TDto">Get entity data transfer object</typeparam>
public partial class EntityDialog<TCreateDto, TUpdateDto, TDto> : EntityComponentBase<TCreateDto, TUpdateDto, TDto>
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }
    private string? _tittle = "";

    /// <inheritdoc />
    protected override Task OnInitializedAsync()
    {
        _tittle = MudDialog?.Title;
        MudDialog?.SetTitle($"{Options.Action} {_tittle}");
        return base.OnInitializedAsync();
    }

    /// <inheritdoc />
    protected override async Task OnCreate()
    {
        await base.OnCreate();
        MudDialog?.SetTitle($"{Options.Action} {_tittle}");
    }
}