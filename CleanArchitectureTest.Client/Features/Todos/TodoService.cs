
using CleanArchitectureTest.Contracts.Entities;
using System.Text.Json;

namespace CleanArchitectureTest.Client.Features.Todos;

public interface ITodoService
{
    Task<List<TodoEntity>> GetTodos();
    Task<List<MemberEntity>> GetAppointeesForTodo(Guid todoId);
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

    public async Task<List<MemberEntity>> GetAppointeesForTodo(Guid todoId)
    {
        try
        {
            var httpResponse = await _client.GetAsync($"appointees?TodoId={todoId}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Error calling appointees at todo enpoint: " + httpResponse.StatusCode);
            }

            var jsonStream = await httpResponse.Content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync<List<MemberEntity>>(jsonStream);

            return data ?? [];
        }
        catch (Exception)
        {

            throw;
        }
    }
}
