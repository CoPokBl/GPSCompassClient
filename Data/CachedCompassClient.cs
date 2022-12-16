using System.Diagnostics;
using System.Text;
using CompassApi;
using GeneralPurposeLib;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace GPSCompassClient.Data; 

public class CachedCompassClient : CompassClient {
    private readonly IJSRuntime _jsRuntime;
    private const int UserProfileCacheTime = 60; // 1 hour
    private const int ClassesCacheTime = 20; // 20 minutes

    public CachedCompassClient(IJSRuntime jsRuntime, string schoolPrefix, Func<string, Task> logger = null) : base(
        schoolPrefix, logger) {
        _jsRuntime = jsRuntime;
    }

    public CachedCompassClient(IJSRuntime jsRuntime, CompassLoginState loginState, Func<string, Task> logger = null) :
        base(loginState, logger) {
        _jsRuntime = jsRuntime;
    }

    public new async Task<CompassUser> GetUserProfile() {
        Stopwatch stopwatch = Stopwatch.StartNew();
        string existingProfile = await GetLocalStorage("cache_user_profile");
        if (existingProfile != "") {
            try {
                existingProfile = Encoding.UTF8.GetString(Convert.FromBase64String(existingProfile));
                (DateTime, CompassUser) usr = JsonConvert.DeserializeObject<(DateTime, CompassUser)>(existingProfile)!;
                if (usr.Item1 < DateTime.Now.AddMinutes(-UserProfileCacheTime)) {
                    throw new Exception("Cached user profile is too old");
                }
                Console.WriteLine("User profile cache load took " + stopwatch.ElapsedMilliseconds + "ms");
                usr.Item2.ThrowIfNull();
                return usr.Item2;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                // Ignore the error and get just the profile
            }
        }
        CompassUser profile = await base.GetUserProfile();
        string profileJson = JsonConvert.SerializeObject((DateTime.Now, profile));
        profileJson = Convert.ToBase64String(Encoding.UTF8.GetBytes(profileJson));
        SetLocalStorage("cache_user_profile", profileJson);
        Console.WriteLine("User profile cache save took " + stopwatch.ElapsedMilliseconds + "ms");
        return profile;
    }

    public new async Task<IEnumerable<CompassClass>> GetClasses(bool getMoreInfo = true, DateTime? startDate = null,
        DateTime? endDate = null, int limit = 25, int page = 1) {
        Console.WriteLine("Getting classes");
        if (startDate != null || endDate != null || limit != 25 || page != 1) {
            Console.WriteLine("Requesting classes with parameters, not using cache");
            return await base.GetClasses(getMoreInfo, startDate, endDate, limit, page);
        }
        
        // If any of the dates aren't null, we can't cache the result because it will be different

        Stopwatch stopwatch = Stopwatch.StartNew();
        string existingClasses = await GetLocalStorage("cache_classes_" + getMoreInfo);
        if (existingClasses != "") {
            try {
                existingClasses = Encoding.UTF8.GetString(Convert.FromBase64String(existingClasses));
                (DateTime, IEnumerable<CompassClass>) classes =
                    JsonConvert.DeserializeObject<(DateTime, IEnumerable<CompassClass>)>(existingClasses)!;
                if (classes.Item1 < DateTime.Now.AddMinutes(-ClassesCacheTime)) {
                    throw new Exception("Cached classes are too old");
                }
                Console.WriteLine("Classes cache load took " + stopwatch.ElapsedMilliseconds + "ms");
                return classes.Item2;
            }
            catch (Exception) {
                // Ignore the error and get just the classes
            }
        }
        CompassClass[] classesFromCompass = (await base.GetClasses(getMoreInfo, startDate, endDate, limit, page)).ToArray();
        string classesJson = JsonConvert.SerializeObject((DateTime.Now, classesFromCompass));
        classesJson = Convert.ToBase64String(Encoding.UTF8.GetBytes(classesJson));
        SetLocalStorage("cache_classes_" + getMoreInfo, classesJson);
        Console.WriteLine("Classes cache save took " + stopwatch.ElapsedMilliseconds + "ms");
        return classesFromCompass;
    }

    private async Task<string> GetLocalStorage(string key) {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
    }

    private async void SetLocalStorage(string key, string value) {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    }
    
}