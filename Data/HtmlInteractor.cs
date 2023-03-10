using Microsoft.JSInterop;

namespace GPSCompassClient.Data; 

// Why the hell is interactor not a word? It feels like it should be.
public class HtmlInteractor {
    private readonly IJSRuntime _jsRuntime;
    
    public HtmlInteractor(IJSRuntime jsRuntime) {
        _jsRuntime = jsRuntime;
    }

    public async Task<string> GetValue(string id) {
        return await _jsRuntime.InvokeAsync<string>("eval", "document.getElementById('" + id + "').value");
    }
    
    public async Task<string> GetHtml(string id) {
        return await _jsRuntime.InvokeAsync<string>("eval", "document.getElementById('" + id + "').innerHTML");
    }
    
    public async Task<string> GetText(string id) {
        return await _jsRuntime.InvokeAsync<string>("eval", "document.getElementById('" + id + "').innerText");
    }
    
    public async Task SetValue(string id, string value) {
        await _jsRuntime.InvokeVoidAsync("eval", "document.getElementById('" + id + "').value = '" + value + "'");
    }
    
    public async Task SetHtml(string id, string value) {
        await _jsRuntime.InvokeVoidAsync("eval", "document.getElementById('" + id + "').innerHTML = '" + value + "'");
    }
    
    public async Task SubmitForm(string id) {
        await _jsRuntime.InvokeVoidAsync("eval", "document.getElementById('" + id + "').submit()");
    }

    public async Task InvokeCode(string code) {
        await _jsRuntime.InvokeVoidAsync("eval", code);
    }
    
    public async Task Log(string msg) {
        await _jsRuntime.InvokeVoidAsync("console.log", msg);
    }

}