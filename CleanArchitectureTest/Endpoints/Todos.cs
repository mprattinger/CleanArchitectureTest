using Carter;
using CleanArchitectureTest.Application.Features.Todos;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureTest.Endpoints;

public class TodosEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/todos")
            .WithTags("Todos")
            .WithOpenApi();

        group.MapGet("/", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllTodos.Query());

            return result.Match(
                m => Results.Ok(m),
                err => Results.BadRequest(err)
                );
        })
        .Produces<List<TodoEntity>>();

        group.MapGet("/{id}", async (Guid Id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetTodoById.Query(Id));

            return result.Match(
                m => Results.Ok(m),
                _ => Results.NotFound(),
                err => Results.BadRequest(err)
                );
        })
            .Produces<TodoEntity>();

        group.MapGet("/createdby", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetTodosByCreator.Query(id));

            return result.Match(
                m => Results.Ok(m),
                _ => Results.NotFound(),
                err => Results.BadRequest(err)
                );
        })
            .Produces<List<TodoEntity>>();

        group.MapPost("/", async ([FromBody] CreateTodoRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateTodo.Command(request.Title, request.Description, request.DueDate, request.CreatedBy));

            return result.Match(
                m => Results.Ok(m),
                err => Results.BadRequest(err)
                );
        })
        .Produces<TodoEntity>();

        group.MapPatch("/{id}", async (Guid Id, [FromBody] ModifyTodoRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new ModifyTodo.Command(Id, request.Title, request.Description, request.DueDate));

            return result.Match(
                m => Results.Ok(m),
                _ => Results.NotFound(),
                err => Results.BadRequest(err)
                );
        })
            .Produces<TodoEntity>();

        group.MapDelete("/{id}", async (Guid Id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteTodo.Command(Id));

            return result.Match(
                _ => Results.Ok(),
                _ => Results.NotFound(),
                err => Results.BadRequest(err)
                );
        });
    }
}
