using System.Collections.ObjectModel;
using System.Linq;
using AvaloniaMVVM.Models;

namespace AvaloniaMVVM.Services;

public class SourceManagementService
{
    private int _nextSourceId = 1;
    private int _nextRequestId = 1;

    public ObservableCollection<ApiSourceModel> ApiSources { get; } = new();

    public ApiSourceModel CreateSource()
    {
        var source = new ApiSourceModel
        {
            ApiSourceId = _nextSourceId,
            Name = $"Source {_nextSourceId}",
            Description = "New source",
            IsExpanded = true
        };

        _nextSourceId++;
        ApiSources.Add(source);

        return source;
    }

    public void DeleteSource(ApiSourceModel source)
    {
        ApiSources.Remove(source);
    }

    public ApiRequestModel CreateRequest(ApiSourceModel source)
    {
        var request = new ApiRequestModel
        {
            ApiRequestId = _nextRequestId,
            ApiSourceId = source.ApiSourceId,
            Name = $"Request {_nextRequestId}",
            HttpMethod = "GET",
            IsActive = true
        };

        _nextRequestId++;
        source.ApiRequests.Add(request);

        return request;
    }

    public void DeleteRequest(ApiSourceModel source, ApiRequestModel request)
    {
        source.ApiRequests.Remove(request);
    }

    public ApiSourceModel? FindSourceForRequest(ApiRequestModel request)
    {
        return ApiSources.FirstOrDefault(source => source.ApiRequests.Contains(request));
    }
}