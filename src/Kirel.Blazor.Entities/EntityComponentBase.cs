using System.Net;
using System.Net.Http.Json;
using AutoMapper;
using Kirel.Blazor.Entities.Models;
using Kirel.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Kirel.Blazor.Entities;

/// <summary>
/// Basic component for editing entity
/// </summary>
/// <typeparam name="TCreateDto">Create entity data transfer object</typeparam>
/// <typeparam name="TUpdateDto">Update entity data transfer object</typeparam>
/// <typeparam name="TDto">Get entity data transfer object</typeparam>
public class EntityComponentBase<TCreateDto, TUpdateDto, TDto> : ComponentBase
{
    [Inject]
    private HttpClient HttpClient { get; set; } = null!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IHttpClientFactory HttpClientFactory { get; set; } = null!;
    [Inject]
    private IMapper Mapper { get; set; } = null!;
    /// <summary>
    /// Dialog content
    /// </summary>
    [Parameter]
    public RenderFragment Content { get; set; }  = null!;
    /// <summary>
    /// Data transfer object for create the entity
    /// </summary>
    [Parameter]
    public TCreateDto? CreateDto  { get; set; }

    /// <summary>
    /// Data transfer object for update the entity
    /// </summary>
    [Parameter]
    public TUpdateDto? UpdateDto { get; set; }
    /// <summary>
    /// Data transfer object for get the entity
    /// </summary>
    [Parameter]
    public TDto? Dto { get; set; }
    /// <summary>
    /// Options for control dialog and fields entity settings
    /// </summary>
    [Parameter]
    public EntityOptions Options { get; set; } = new () { Action = EntityAction.Create };

    /// <summary>
    /// Http client instance name
    /// </summary>
    [Parameter]
    public string HttpClientName { get; set; } = "";

    /// <summary>
    /// Relative url to API endpoint
    /// </summary>
    [Parameter]
    public string? HttpRelativeUrl { get; set; }
    /// <summary>
    /// Before create request event handler
    /// </summary>
    [Parameter]
    public Func<TCreateDto?, Task>? BeforeCreateRequest { get; set; }
    /// <summary>
    /// Before update request event handler
    /// </summary>
    [Parameter]
    public Func<TUpdateDto?, Task>? BeforeUpdateRequest { get; set; }
    /// <summary>
    /// After entity data transfer object received event handler
    /// </summary>
    [Parameter]
    public Func<TDto?, Task>? AfterDtoReceived { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        HttpClient = HttpClientFactory.CreateClient(HttpClientName);
        Mapper.Map(Dto, UpdateDto);
        await base.OnInitializedAsync();
    }

    private async Task ShowValidationErrors(HttpResponseMessage resp)
    {
        var validationDto = await resp.Content.ReadFromJsonAsync<ValidationErrorsDto>();
        foreach (var filedAndMessages in validationDto!.Errors)
        {
            foreach (var message in filedAndMessages.Value)
            {
                Snackbar.Add($"{filedAndMessages.Key}: {message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Method for create button
    /// </summary>
    protected virtual async Task OnCreate()
    {
        BeforeCreateRequest?.Invoke(CreateDto);
        var resp = await HttpClient.PostAsJsonAsync(HttpRelativeUrl, CreateDto);
        if (resp.IsSuccessStatusCode)
        {
            var respDto = await resp.Content.ReadFromJsonAsync<TDto>();
            if (Dto != null)
                Mapper.Map(respDto, Dto);
            if (UpdateDto != null)
            {
                Mapper.Map(respDto, UpdateDto);
                Options.Action = EntityAction.Edit;
            } 
            else
                Options.Action = EntityAction.Read;
            
            Snackbar.Add($"Successfully created!", Severity.Success);
        }
        else if (resp.StatusCode == HttpStatusCode.BadRequest)
        {
            await ShowValidationErrors(resp);
        }
    }
    
    /// <summary>
    /// Method for update button
    /// </summary>
    protected virtual async Task OnUpdate()
    {
        BeforeUpdateRequest?.Invoke(UpdateDto);
        var idProp = Dto?.GetType().GetProperties().FirstOrDefault(p => p.Name == "Id");
        var id = idProp?.GetValue(Dto)?.ToString();
        var resp = await HttpClient.PutAsJsonAsync($"{HttpRelativeUrl}/{id}", UpdateDto);
        if (resp.IsSuccessStatusCode)
        {
            var respDto = await resp.Content.ReadFromJsonAsync<TDto>();
            AfterDtoReceived?.Invoke(respDto);
            if (Dto != null)
                Mapper.Map(respDto, Dto);
            if (UpdateDto != null)
                Mapper.Map(respDto, UpdateDto);

            Snackbar.Add($"Successfully updated!", Severity.Success);
        } 
        else if (resp.StatusCode == HttpStatusCode.BadRequest)
        {
            await ShowValidationErrors(resp);
        }
    }
}