using System.Reflection;

namespace Kirel.Blazor.Entities.Models;

/// <summary>
/// A class that provides access to a field in all dtos at once. Used to synchronize data.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class EntityDtoProperty<T>
{
    private readonly string _propName;
    private readonly object? _createDto;
    private readonly object? _updateDto;
    private readonly object? _dto;
    /// <summary>
    /// Creates instance of a class that provides access to a field in all dtos at once.
    /// </summary>
    /// <param name="propName">Property name</param>
    /// <param name="createDto">Entity create data transfer object</param>
    /// <param name="updateDto">Entity update data transfer object</param>
    /// <param name="dto">Entity get data transfer object</param>
    public EntityDtoProperty(string propName, object? createDto, object? updateDto, object? dto)
    {
        _propName = propName;
        _createDto = createDto;
        _updateDto = updateDto;
        _dto = dto;
    }

    private T? ToType(object? value)
    {
        if (value != null)
            return (T)value;
        return default;
    }

    private PropertyInfo? GetPropertyInfo(object? dto)
    {
        return dto?.GetType().GetProperties().FirstOrDefault(pr => pr.Name == _propName);
    }

    private T? GetPropertyValueFrom(object? dto)
    {
        var propDto = GetPropertyInfo(dto);
        return ToType(propDto?.GetValue(dto));
    }

    private void SetPropertyValueTo(T? value, object? dto)
    {
        var propInfo = GetPropertyInfo(dto);
        if (propInfo != null) 
            propInfo.SetValue(dto, value);
    }
    
    /// <summary>
    /// Provides a getter and setter to property in all dtos, if in dtos, this field exists.
    /// </summary>
    public T? Property
    {
        get
        {
            var val = GetPropertyValueFrom(_dto);
            if (val != null)
                return val;
            val = GetPropertyValueFrom(_updateDto);
            if (val != null)
                return val;
            val = GetPropertyValueFrom(_createDto);
            if (val != null)
                return val;
            
            return default;
        }
        set
        {
            SetPropertyValueTo(value, _dto);
            SetPropertyValueTo(value, _createDto);
            SetPropertyValueTo(value, _updateDto);
        }
    }
}