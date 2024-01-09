using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Data;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchitectureTest.Application.Features.Todos
{
    public class GetAllTodos
    {
        public record Query : IRequest<OneOf<List<TodoEntity>, ApplicationError>>;

        internal sealed class Handler : IRequestHandler<Query, OneOf<List<TodoEntity>, ApplicationError>>
        {
            private readonly ApplicationContext _context;

            public Handler(ApplicationContext context)
            {
                _context = context;
            }

            public async Task<OneOf<List<TodoEntity>, ApplicationError>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var todos = await _context
                        .Todos
                        .Include(x => x.CreatedBy)
                        .ToListAsync(cancellationToken);

                    return todos.Select(t => t.Adapt<TodoEntity>()).ToList();
                }
                catch (Exception ex)
                {
                    return ApplicationError.ApplicationError_Exception(nameof(GetAllTodos), ex);
                }
            }
        }
    }
}
