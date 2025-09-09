namespace SmartPdM.Services.Storage;

public class PreferencesStore : ILocalStore
{
    public void SetString(string key, string value) => Preferences.Default.Set(key, value);
    public string? GetString(string key) => Preferences.Default.Get<string?>(key, null);
    public void Remove(string key) => Preferences.Default.Remove(key);
}