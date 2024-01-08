using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Contracts.Responses;
using CleanArchitectureTest.Data;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchitectureTest.Application.Features.Appointees;

public class ModifyAppointee
{
    public record Command(Guid todoId, Guid oldMemberId, Guid newMemberId) : IRequest<OneOf<TodoAppointeeEntity, AppointeeNotFound, ApplicationError>>;

    internal sealed class Handler : IRequestHandler<Command, OneOf<TodoAppointeeEntity, AppointeeNotFound, ApplicationError>>
    {
        private readonly ApplicationContext _context;

        public Handler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<OneOf<TodoAppointeeEntity, AppointeeNotFound, ApplicationError>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var todo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == request.todoId, cancellationToken);
                if (todo is null)
                {
                    return new AppointeeNotFound("Todo not found!");
                }

                var ta = await _context.TodoAppointees.FirstOrDefaultAsync(x => x.TodoId == request.todoId
                    && x.AppointeeId == request.oldMemberId, cancellationToken);
                if (ta is null)
                {
                    return new AppointeeNotFound("TodoAppointee not found!");
                }

                var appointee = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.newMemberId, cancellationToken);
                if (appointee is null)
                {
                    return new AppointeeNotFound("New Member not found!");
                }

                ta.AppointeeId = request.newMemberId;
                ta.Appointee = appointee;

                _context.Update(ta);
                await _context.SaveChangesAsync();

                return new TodoAppointeeEntity
                {
                    Todo = ta.Todo.Adapt<TodoEntity>(),
                    Appointee = ta.Appointee.Adapt<MemberEntity>()
                };
            }
            catch (Exception ex)
            {
                return ApplicationError.ApplicationError_Exception(nameof(ModifyAppointee), ex);
            }
        }
    }
}
