using Blazored.SessionStorage;
using System.Text.Json;

namespace ShopPhone.Client.Auth;

public static class SessionStorageExtension
{
    public static async Task SaveStorage<T>(this ISessionStorageService sessionStorageService, string key, T item) where T : class
    {
        var json = JsonSerializer.Serialize(item);
        await sessionStorageService.SetItemAsStringAsync(key, json);
    }

    public static async Task<T?> GetStorage<T>(this ISessionStorageService sessionStorageService, string key)
    where T : class
    {
        var json = await sessionStorageService.GetItemAsStringAsync(key);
        if (json is null)
            return null;

        return JsonSerializer.Deserialize<T>(json);
    }
}