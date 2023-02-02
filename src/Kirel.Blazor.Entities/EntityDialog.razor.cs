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
where TCreateDto : new()
where TUpdateDto : new()
where TDto : new()
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }
    private string? _tittle = "";

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        _tittle = MudDialog?.Title;
        await base.OnInitializedAsync().ContinueWith(_ => MudDialog?.SetTitle($"{Options?.Action} {_tittle}"));;
    }

    /// <inheritdoc />
    protected override async Task OnCreate()
    {
        await base.OnCreate().ContinueWith(_ => MudDialog?.SetTitle($"{Options?.Action} {_tittle}"));
    }
    /// <inheritdoc />
    protected override async Task OnUpdate()
    {
        await base.OnUpdate().ContinueWith(_ => MudDialog?.SetTitle($"{Options?.Action} {_tittle}"));
    }
}