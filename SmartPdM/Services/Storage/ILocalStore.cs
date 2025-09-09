namespace SmartPdM.Services.Storage;

public interface ILocalStore
{
    void SetString(string key, string value);
    string? GetString(string key);
    void Remove(string key);
}