using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pinguin.Models;

public static class UpdateChecker
{
    private static HttpClient _client = new()
    {
        BaseAddress = new Uri("https://api.github.com"),
        DefaultRequestHeaders = { {"User-Agent", "Pinguin"} }
    };

    public static async Task<bool> CheckForUpdates()
    {
        using HttpResponseMessage response = await _client.GetAsync("/repos/kindaconfusion/Pinguin/releases");
        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var releases = JsonConvert.DeserializeObject<List<Release>>(jsonResponse);
        var version = Assembly.GetEntryAssembly()?.GetName().Version.ToString();
        version = version.Substring(0, version.Length - 2);
        var thisRelease = releases.FirstOrDefault(r => r.tag_name == version);
        if (thisRelease == null) return false;
        var newestRelease = releases.OrderByDescending(r => r.tag_name).First();
        return thisRelease.tag_name != newestRelease.tag_name;
    }
}

public class Release
{
    public string url { get; set; }
    public string tag_name { get; set; }
    public string name { get; set; }
    public bool prerelease { get; set; }
    public List<Asset> assets { get; set; }
    public string body { get; set; }
}

public class Asset
{
    public string url { get; set; }
    public string name { get; set; }
    public object label { get; set; }
    public string state { get; set; }
    public string browser_download_url { get; set; }
}