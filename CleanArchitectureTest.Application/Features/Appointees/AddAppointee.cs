using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Contracts.Responses;
using CleanArchitectureTest.Data;
using CleanArchitectureTest.Data.DTOs;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchitectureTest.Application.Features.Appointees;

public class AddAppointee
{
    public record Command(Guid todoId, Guid memberId) : IRequest<OneOf<TodoAppointeeEntity, AppointeeNotFound, ApplicationError>>;

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

                var appointee = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.memberId, cancellationToken);
                if (appointee is null)
                {
                    return new AppointeeNotFound("Member not found!");
                }

                var ta = new TodoAppointee
                {
                    Todo = todo,
                    Appointee = appointee,
                };

                _context.TodoAppointees.Add(ta);
                await _context.SaveChangesAsync(cancellationToken);

                return new TodoAppointeeEntity
                {
                    Todo = ta.Todo.Adapt<TodoEntity>(),
                    Appointee = ta.Appointee.Adapt<MemberEntity>()
                };
            }
            catch (Exception ex)
            {
                return ApplicationError.ApplicationError_Exception(nameof(AddAppointee), ex);
            }
        }
    }
}
