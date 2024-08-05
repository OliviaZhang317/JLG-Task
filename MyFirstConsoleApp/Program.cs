// See https://aka.ms/new-console-template for more information

using Microsoft.Playwright;
using System.Threading.Tasks;
using System.Reflection.Emit;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using System.Text;


// Initialize Playwright and launch browser
using var playwright = await Playwright.CreateAsync();
await using var browser = await playwright.Chromium.LaunchAsync();
var page = await browser.NewPageAsync();

// Navigate to the page
await page.GotoAsync("https://demo.snipeitapp.com/login");
await page.ScreenshotAsync(new()
{
    Path = "screenshot.png"
});

// Login
await page.GetByLabel("username").FillAsync("admin");
await page.GetByLabel("password").FillAsync("password");

await page.Keyboard.PressAsync("Tab");
await page.Keyboard.PressAsync("Tab");
await page.Keyboard.PressAsync("Enter");


await page.WaitForNavigationAsync();
Console.WriteLine("Form submitted!");

await page.ScreenshotAsync(new()
{
    Path = "screenshot3.png"
});

//If the dropdown toggle has unique text content, 
// use a text-based selector to target it more precisely:
var dropdown = page.Locator("a.dropdown-toggle:has-text('Create New')");
await dropdown.ClickAsync();

//choose Asset
var option = page.Locator("ul.dropdown-menu a[href='https://demo.snipeitapp.com/hardware/create']");
await option.ClickAsync();


await page.ScreenshotAsync(new()
{
    Path = "screenshot5.png"
});

//get asset tag value for later navigation
var inputLocator = page.Locator("input#asset_tag");
string assettagValue = await inputLocator.GetAttributeAsync("value");
Console.WriteLine($"asset_tag is: {assettagValue}");


// Select model
var model = page.Locator("#select2-model_select_id-container");
await model.ClickAsync();


var modeloptionToSelect = page.Locator(".select2-results__option:has-text('Laptops - Macbook Pro 13')");


await page.ScreenshotAsync(new()
{
    Path = "screenshot6.png"
});


//await modeloptionsLocator.ScrollIntoViewIfNeededAsync();
//await modeloptionsLocator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });

await modeloptionToSelect.ClickAsync();

await page.ScreenshotAsync(new()
{
    Path = "screenshot7.png"
});

string modelselectedValue = await page.Locator("#select2-model_select_id-container").GetAttributeAsync("title");
Console.WriteLine($"Model selected option value: {modelselectedValue}");

// Select status
var status = page.Locator("#select2-status_select_id-container");
await status.ClickAsync();

await page.ScreenshotAsync(new()
{
    Path = "screenshot9.png"
});


await page.ScreenshotAsync(new()
{
    Path = "screenshot10.png"
});

 
var statusoptionToSelect = page.Locator(".select2-results__option:has-text('Ready to Deploy')");

await statusoptionToSelect.ClickAsync();

await page.ScreenshotAsync(new()
{
    Path = "screenshot11.png"
});

string statusselectedValue = await page.Locator("#select2-status_select_id-container").GetAttributeAsync("title");
Console.WriteLine($"Status selected option value: {statusselectedValue}");


await Task.Delay(2000); // Delay for 2 seconds


// Select user
var user = page.Locator("#select2-assigned_user_select-container");
await user.ClickAsync();
await Task.Delay(2000); // Delay for 2 seconds
await page.ScreenshotAsync(new()
{
    Path = "screenshot12.png"
});


// Select a random option
//int optionCount = await useroptionsLocator.CountAsync();
int optionCount = await page.Locator(".select2-results__option").CountAsync();
Console.WriteLine($"option count: {optionCount}");

var random = new Random();
int randomIndex = random.Next(optionCount);
Console.WriteLine($"randomIndex: {randomIndex}");

var optionToSelect = await page.Locator(".select2-results__option").Nth(randomIndex).TextContentAsync();

Console.WriteLine($"User Selecting option: {optionToSelect}");

var useroptionToSelect = page.Locator($".select2-results__option:has-text('{optionToSelect}')");


await useroptionToSelect.ClickAsync();

await page.ScreenshotAsync(new()
{
    Path = "screenshot14.png"
});

string userselectedValue = await page.Locator("#select2-assigned_user_select-container").GetAttributeAsync("title");
Console.WriteLine($"User selected option value: {userselectedValue}");

//save button
var savebutton = page.Locator("div.box-header.with-border div.col-md-3 button.btn.btn-primary.pull-right"); 
await savebutton.ClickAsync();
await Task.Delay(2000); // Delay for 8 seconds

await page.ScreenshotAsync(new()
{
    Path = "screenshot15.png"
});

await Task.Delay(2000); // Delay for 8 seconds


//select asset list
var assetlist = page.Locator("ul.nav.navbar-nav a[href='https://demo.snipeitapp.com/hardware']");
await assetlist.ClickAsync();

await page.ScreenshotAsync(new()
{
    Path = "screenshot16.png"
});

//search for created asset by asset tag
var search = page.Locator(".form-control.search-input");


await search.FillAsync($"{assettagValue}");
await page.Keyboard.PressAsync("Enter");


await page.ScreenshotAsync(new()
{
    Path = "screenshot17.png"
});


//click on the asset tag to check detailed info
var assetLocator = page.Locator($"table tbody tr td a:has-text('{assettagValue}')");
await assetLocator.ClickAsync();

await page.ScreenshotAsync(new()
{
    Path = "screenshot18.png"
});

//check the content in Status row 
var statuslabelLocator = page.Locator("strong:has-text('Status')");
var parentRowLocator = statuslabelLocator.Locator("..>>..");
var statusTextLocator = parentRowLocator.Locator("div.col-md-9");
string statusText = (await statusTextLocator.TextContentAsync()).Trim();
Console.WriteLine($"Status text: {statusText}");

string trimmedstatusselectedValue = statusselectedValue.Trim();
Console.WriteLine($"Trimmed statusselectedValue: \"{trimmedstatusselectedValue}\"");

if (statusText.Contains(trimmedstatusselectedValue))
{
    Console.WriteLine("Status is correct.");
}
else
{
    Console.WriteLine("Status is not correct.");
}


string trimmeduserselectedValue = userselectedValue.Trim();
Console.WriteLine($"Trimmed userselectedValue: \"{trimmeduserselectedValue}\"");

if (statusText.Contains(trimmeduserselectedValue))
{
    Console.WriteLine("User is correct.");
}
else
{
    Console.WriteLine("User is not correct.");
}

//check the content in Model No row
var modelLabelLocator = page.Locator("strong:has-text('Model No')");
await modelLabelLocator.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 60000 // Adjust timeout as needed
        });

var modelparentRowLocator = modelLabelLocator.Locator("..>>..");
var modelTextLocator = modelparentRowLocator.Locator("div.col-md-9");
await modelTextLocator.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 60000 // Adjust timeout as needed
        });
string modelText = (await modelTextLocator.TextContentAsync()).Trim();
Console.WriteLine($"Model text: {modelText}");


string pattern2 = @"\(#" + Regex.Escape(modelText) + @"\)";
if (Regex.IsMatch(modelselectedValue, pattern2))
{
    Console.WriteLine("Model is correct.");
}
else
{
    Console.WriteLine("Model is not correct.");
}

//go to history page
var historyLocator = page.Locator("span.hidden-xs.hidden-sm:has-text('History')");
await historyLocator.ClickAsync();

await page.ScreenshotAsync(new()
{
    Path = "screenshot20.png"
});

