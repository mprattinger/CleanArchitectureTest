using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Data;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace CleanArchitectureTest.Application.Features.Todos;

public class GetAppointeesForTodo
{
    public record Query(Guid todoId) : IRequest<OneOf<List<MemberEntity>, NotFound, ApplicationError>>;

    internal sealed class Handler : IRequestHandler<Query, OneOf<List<MemberEntity>, NotFound, ApplicationError>>
    {
        private readonly ApplicationContext _context;

        public Handler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<OneOf<List<MemberEntity>, NotFound, ApplicationError>> Handle(Query request, CancellationToken cancellationToken)
        {
            try
            {
                var ta = await _context.TodoAppointees.Where(x => x.TodoId == request.todoId).ToListAsync(cancellationToken);
                if (ta is null)
                {
                    return new NotFound();
                }

                var ret = _context
                    .Members
                    .Where(x => ta.Select(x => x.AppointeeId).Contains(x.Id))
                    .Select(x => x.Adapt<MemberEntity>())
                    .ToList();

                return ret;
            }
            catch (Exception ex)
            {
                return ApplicationError.ApplicationError_Exception(nameof(GetAppointeesForTodo), ex);
            }
        }
    }
}
