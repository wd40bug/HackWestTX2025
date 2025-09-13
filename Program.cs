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
DateTime message1_self = new DateTime(100000000);
DateTime message2_other = new DateTime(200000000);
DateTime message3_self = new DateTime(300000000);
Message aMessage1 = new Message(message1_self, "Hello", true);
Message aMessage2 = new Message(message2_other, "<3 Hello, 😉😉😉😉heyyyyyyyy <3 <3 <3", false);
Message aMessage3 = new Message(message3_self, "Hello again", true);
List<Message> log = new List<Message>() { aMessage1, aMessage2, aMessage3 };

ChatLog aLog = new ChatLog(log);
Console.WriteLine(aLog.TimeBetweenResponse(2));

Console.WriteLine(aLog.FindAverageResponseTime());


app.Run();
