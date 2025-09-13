using BlazorTest.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
Console.WriteLine("test");
DateTime message1 = new DateTime(100000);
DateTime message2 = new DateTime(234235345);
Message aMessage = new Message(message1, "Hello", true);
Message anotherMessage = new Message(message2, "Hello,", false);
List<Message> log = new List<Message>() { aMessage, anotherMessage };

ChatLog aLog = new ChatLog(log);
Console.WriteLine(aLog.TimeBetweenResponse(0));

Console.WriteLine(aLog.FindAverageResponseTime());


app.Run();
