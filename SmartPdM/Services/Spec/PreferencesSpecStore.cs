using SmartPdM.Models;

namespace SmartPdM.Services.Spec;

public class PreferencesSpecStore : ISpecStore
{
    const string K = "SmartPdM.InspectionSpec.v1";

    public Task<InspectionSpec> LoadAsync()
    {
        var json = Preferences.Default.Get<string?>(K, null);
        if (string.IsNullOrWhiteSpace(json))
            return Task.FromResult(new InspectionSpec()); // 기본값
        try
        {
            var spec = System.Text.Json.JsonSerializer.Deserialize<InspectionSpec>(json)
                       ?? new InspectionSpec();
            return Task.FromResult(spec);
        }
        catch
        {
            return Task.FromResult(new InspectionSpec());
        }
    }

    public Task SaveAsync(InspectionSpec spec)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(spec);
        Preferences.Default.Set(K, json);
        return Task.CompletedTask;
    }

    public Task ResetAsync()
    {
        Preferences.Default.Remove(K);
        return Task.CompletedTask;
    }
}
