using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Contracts.Responses;
using CleanArchitectureTest.Data;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchitectureTest.Application.Features.Members;

public class GetTodosForMember
{
    public record Query(Guid memberId) : IRequest<OneOf<List<TodoEntity>, AppointeeNotFound, ApplicationError>>;

    internal sealed class Handler : IRequestHandler<Query, OneOf<List<TodoEntity>, AppointeeNotFound, ApplicationError>>
    {
        private readonly ApplicationContext _context;

        public Handler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<OneOf<List<TodoEntity>, AppointeeNotFound, ApplicationError>> Handle(Query request, CancellationToken cancellationToken)
        {
            try
            {
                var appointee = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.memberId, cancellationToken);
                if (appointee is null)
                {
                    return new AppointeeNotFound("Member not found!");
                }

                var links = await _context.TodoAppointees.Where(x => x.AppointeeId == appointee.Id).ToListAsync(cancellationToken);

                return links.Select(x => x.Todo.Adapt<TodoEntity>()).ToList();
            }
            catch (Exception ex)
            {
                return ApplicationError.ApplicationError_Exception(nameof(GetTodosForMember), ex);
            }
        }
    }
}
