using BlazorMenuCommon.DTOs;

namespace BlazorMenu.Services
{
    public interface R_IMenuService
    {
        Task<List<MenuListDTO>> GetMenuAsync();
    }
}