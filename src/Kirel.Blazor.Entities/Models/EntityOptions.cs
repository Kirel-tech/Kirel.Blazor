namespace Kirel.Blazor.Entities.Models;

/// <summary>
/// Options for control dialog and fields entity settings
/// </summary>
public class EntityOptions
{
    /// <summary>
    /// Enumerated entity action
    /// </summary>
    public EntityAction Action { get; set; } = EntityAction.Read;
    /// <summary>
    /// Ignored properties for auto property creation
    /// </summary>
    public List<string> IgnoredProperties { get; set; } = new ();
}