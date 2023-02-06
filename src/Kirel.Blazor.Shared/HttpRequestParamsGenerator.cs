using System.Text;
using MudBlazor;

namespace Kirel.Blazor.Shared;

/// <summary>
/// Http requests parameters generator for basic requests
/// </summary>
public static class HttpRequestParamsGenerator
{
    /// <summary>
    /// Generate parameters for get list requests.
    /// </summary>
    /// <param name="state">MudBlazor table state</param>
    /// <returns>Parameters string</returns>
    public static string GetListParams(TableState state)
    {
        var uriBuilder = new StringBuilder();
        if (!string.IsNullOrEmpty(state?.SortLabel))
        {
            uriBuilder.Append($"&orderBy={state.SortLabel}");
            uriBuilder.Append(state.SortDirection == SortDirection.Descending ? "&orderDirection=desc" : "&orderDirection=asc");
        }

        uriBuilder.Append($"&pageNumber={state!.Page + 1}");
        uriBuilder.Append($"&pageSize={state!.PageSize}");
        
        var uriStr = uriBuilder.ToString();
        return uriStr.Remove(0, 1);
    }
}