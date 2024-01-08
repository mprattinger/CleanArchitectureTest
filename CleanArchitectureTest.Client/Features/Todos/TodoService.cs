
using CleanArchitectureTest.Contracts.Entities;
using System.Text.Json;

namespace CleanArchitectureTest.Client.Features.Todos;

public interface ITodoService
{
    Task<List<TodoEntity>> GetTodos();
}

public class TodoService : ITodoService
{
    private readonly HttpClient _client;

    public TodoService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("todos");
    }

    public async Task<List<TodoEntity>> GetTodos()
    {
        try
        {
            var httpResponse = await _client.GetAsync("");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Error calling todos at todo enpoint: " + httpResponse.StatusCode);
            }

            var jsonStream = await httpResponse.Content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync<List<TodoEntity>>(jsonStream);

            return data ?? [];
        }
        catch (Exception)
        {

            throw;
        }
    }
}
