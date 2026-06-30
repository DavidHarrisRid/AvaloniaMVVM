using System.Collections.Generic;
using AvaloniaMVVM.Models;

namespace AvaloniaMVVM.Services;

public class SourceManagementService
{
    public List<ApiSourceModel> ApiSources { get; private set; }
    
    public void CreateSource()
    {
        ApiSources.Add(new ApiSourceModel(){});
    }
}