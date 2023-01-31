using System.Reflection;
using Kirel.Blazor.Entities.Models;
using Microsoft.AspNetCore.Components;

namespace Kirel.Blazor.Entities;

/// <summary>
/// Basic component for generating list of properties based on all dtos and binding multiple dtos properties to blazor elements.
/// </summary>
/// <typeparam name="TCreateDto">Create entity data transfer object</typeparam>
/// <typeparam name="TUpdateDto">Update entity data transfer object</typeparam>
/// <typeparam name="TDto">Get entity data transfer object</typeparam>
public partial class EntityProperties<TCreateDto, TUpdateDto, TDto> : ComponentBase
{
    private readonly Dictionary<EntityAction, List<string>> _readonlyPropNames = new ();
    private List<PropertyInfo> _allProperties = new ();
    
    /// <summary>
    /// Options for control dialog and fields entity settings
    /// </summary>
    [Parameter] public EntityOptions Options { get; set; } = new ();
    /// <summary>
    /// Create entity data transfer object
    /// </summary>
    [Parameter] public TCreateDto? CreateDto { get; set; }
    /// <summary>
    /// Update entity data transfer object
    /// </summary>
    [Parameter] public TUpdateDto? UpdateDto { get; set; }
    /// <summary>
    /// Get entity data transfer object
    /// </summary>
    [Parameter] public TDto? Dto { get; set; }

    /// <inheritdoc />
    protected override Task OnInitializedAsync()
    {
        _allProperties = Dto?.GetType().GetProperties()
            .Where(p => !Options.IgnoredProperties.Contains(p.Name)).ToList() ?? new List<PropertyInfo>();
        
        var createDtoProperties = CreateDto?.GetType().GetProperties()
            .Where(p => !Options.IgnoredProperties.Contains(p.Name)).ToList() ?? new List<PropertyInfo>();
        var updateDtoProperties = UpdateDto?.GetType().GetProperties()
            .Where(p => !Options.IgnoredProperties.Contains(p.Name)).ToList() ?? new List<PropertyInfo>();
        
        _allProperties.AddRange(createDtoProperties
            .Where(c => !_allProperties.Select(p => p.Name).Contains(c.Name)));
        _allProperties.AddRange(updateDtoProperties
            .Where(c => !_allProperties.Select(p => p.Name).Contains(c.Name)));
        
        var updateDtoPropNames = updateDtoProperties.Select(p => p.Name);
        var createDtoPropNames = createDtoProperties.Select(p => p.Name);
        _readonlyPropNames.Add(EntityAction.Edit, _allProperties
            .Where(p => !updateDtoPropNames.Contains(p.Name))
            .Select(p => p.Name)
            .ToList());
        _readonlyPropNames.Add(EntityAction.Create, _allProperties
            .Where(p => !createDtoPropNames.Contains(p.Name))
            .Select(p => p.Name)
            .ToList());
        _readonlyPropNames.Add(EntityAction.Read, _allProperties
            .Select(p => p.Name)
            .ToList());
        
        return base.OnInitializedAsync();
    }
}