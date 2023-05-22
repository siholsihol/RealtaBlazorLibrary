using BlazorMenuCommon.DTOs;

namespace BlazorMenu.Services
{
    public interface R_IMenuService
    {
        Dictionary<string, string[]> MenuAccess { get; }
        string[] MenuIdList { get; }

        Task<List<MenuListDTO>> GetMenuAsync();
        Task SetMenuAccessAsync();
    }
}