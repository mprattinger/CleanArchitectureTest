using CleanArchitectureTest.Client.Features.Todos;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var baseUrl = new Uri(builder.HostEnvironment.BaseAddress);

builder.Services.AddHttpClient("todos", (x =>
{
    x.BaseAddress = new Uri(baseUrl, "api/todos/");
}));

builder.Services.AddScoped<ITodoService, TodoService>();

await builder.Build().RunAsync();