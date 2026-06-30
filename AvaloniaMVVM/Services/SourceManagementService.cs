using System.Collections.ObjectModel;
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
            Description = "New source"
        };

        _nextSourceId++;
        ApiSources.Add(source);

        return source;
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
}