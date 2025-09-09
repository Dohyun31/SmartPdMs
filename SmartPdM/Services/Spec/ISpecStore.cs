using SmartPdM.Models;

namespace SmartPdM.Services.Spec;

public interface ISpecStore
{
    Task<InspectionSpec> LoadAsync();
    Task SaveAsync(InspectionSpec spec);
    Task ResetAsync(); // 기본값으로 초기화
}
