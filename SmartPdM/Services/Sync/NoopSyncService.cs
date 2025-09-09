namespace SmartPdM.Services.Sync;

public class NoopSyncService : ISyncService
{
    public Task SyncAsync() => Task.CompletedTask; // 나중에 서버 붙이면 구현
}