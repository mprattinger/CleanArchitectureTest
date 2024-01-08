using Carter;
using CleanArchitectureTest.Application.Features.Appointees;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureTest.Endpoints;

public class AppointeesEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/apointees")
             .WithTags("Appointees")
             .WithOpenApi();

        group.MapPost("/", async ([FromBody] AddOrRemoveAppointeeRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new AddAppointee.Command(request.todoId, request.memberId));

            return result.Match(
                a => Results.Ok(a),
                nf => Results.NotFound(nf.msg),
                err => Results.BadRequest(err)
                );
        })
            .Produces<TodoAppointeeEntity>();

        group.MapPatch("/{id}", async (Guid id, [FromBody] ModifyAppointee.Command command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);

            return result.Match(
                a => Results.Ok(a),
                nf => Results.NotFound(nf.msg),
                err => Results.BadRequest(err)
                );
        })
            .Produces<TodoAppointeeEntity>();

        group.MapDelete("/", async ([FromBody] AddOrRemoveAppointeeRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteAppointee.Command(request.todoId, request.memberId));

            return result.Match(
                _ => Results.Ok(),
                nf => Results.NotFound(nf.msg),
                err => Results.BadRequest(err)
                );
        });
    }
}
