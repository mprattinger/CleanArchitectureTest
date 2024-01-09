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

        group.MapGet("/{id}", async (Guid TodoId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetTodoById.Query(TodoId));

            return result.Match(
                m => Results.Ok(m),
                _ => Results.NotFound(),
                err => Results.BadRequest(err)
                );
        })
            .Produces<TodoEntity>();

        group.MapGet("/createdby", async (Guid CreatedById, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetTodosByCreator.Query(CreatedById));

            return result.Match(
                m => Results.Ok(m),
                _ => Results.NotFound(),
                err => Results.BadRequest(err)
                );
        })
            .Produces<List<TodoEntity>>();

        group.MapGet("/appointees", async (Guid TodoId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAppointeesForTodo.Query(TodoId));

            return result.Match(
                m => Results.Ok(m),
                _ => Results.NotFound(),
                err => Results.BadRequest(err)
                );
        })
            .Produces<List<MemberEntity>>();

        group.MapPost("/", async ([FromBody] CreateTodoRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateTodo.Command(request.Title, request.Description, request.DueDate, request.CreatedBy));

            return result.Match(
                m => Results.Ok(m),
                _ => Results.NotFound(),
                err => Results.BadRequest(err)
                );
        })
        .Produces<TodoEntity>();

        group.MapPatch("/{id}", async (Guid TodoId, [FromBody] ModifyTodoRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new ModifyTodo.Command(TodoId, request.Title, request.Description, request.DueDate));

            return result.Match(
                m => Results.Ok(m),
                _ => Results.NotFound(),
                err => Results.BadRequest(err)
                );
        })
            .Produces<TodoEntity>();

        group.MapDelete("/{id}", async (Guid TodoId, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteTodo.Command(TodoId));

            return result.Match(
                _ => Results.Ok(),
                _ => Results.NotFound(),
                err => Results.BadRequest(err)
                );
        });
    }
}
