namespace Kirel.Blazor.Entities;

/// <summary>
/// MudBlazor entity page for viewing and editing entity
/// </summary>
/// <typeparam name="TCreateDto">Create entity data transfer object</typeparam>
/// <typeparam name="TUpdateDto">Update entity data transfer object</typeparam>
/// <typeparam name="TDto">Get entity data transfer object</typeparam>
public partial class EntityPage<TCreateDto, TUpdateDto, TDto> : EntityComponentBase<TCreateDto, TUpdateDto, TDto>
where TCreateDto : new()
where TUpdateDto : new()
where TDto : new()
{
    
}