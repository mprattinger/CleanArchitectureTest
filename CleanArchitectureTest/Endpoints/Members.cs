using Carter;
using CleanArchitectureTest.Application.Features.Members;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureTest.Endpoints;

public class MembersEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/members")
            .WithTags("Members")
            .WithOpenApi();

        group.MapGet("/", async (IMediator mediator) =>
        {
            var members = await mediator.Send(new GetAllMembers.Query());

            Results.Ok(members);
        })
        .Produces<List<MemberEntity>>();

        group.MapGet("/{id}", async (Guid Id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetMemberById.Query(Id));

            return result.Match(
                m => Results.Ok(m),
                _ => Results.NotFound(),
                err => Results.BadRequest(err)
                );
        })
            .Produces<MemberEntity>();

        group.MapPost("/", async ([FromBody] CreateMember.Command command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);

            return result.Match(
                m => Results.Ok(m),
                err => Results.BadRequest(err)
                );
        })
        .Produces<MemberEntity>();

        group.MapPatch("/{id}", async (Guid Id, [FromBody] ModifyMemberRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new ModifyMember.Command(Id, request.FirstName, request.LastName));

            return result.Match(
                m => Results.Ok(m),
                _ => Results.NotFound(),
                err => Results.BadRequest(err)
                );
        })
            .Produces<MemberEntity>();

        group.MapDelete("/{id}", async (Guid Id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteMember.Command(Id));

            return result.Match(
                _ => Results.Ok(),
                _ => Results.NotFound(),
                err => Results.BadRequest(err)
                );
        });
    }
}