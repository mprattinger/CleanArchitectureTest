using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Data;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace CleanArchitectureTest.Application.Features.Todos;

public class GetTodosByCreator
{
    public record Query(Guid creatorId) : IRequest<OneOf<List<TodoEntity>, NotFound, ApplicationError>>;

    internal sealed class Handler : IRequestHandler<Query, OneOf<List<TodoEntity>, NotFound, ApplicationError>>
    {
        private readonly ApplicationContext _context;

        public Handler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<OneOf<List<TodoEntity>, NotFound, ApplicationError>> Handle(Query request, CancellationToken cancellationToken)
        {
            try
            {
                var todos = await _context.Todos.Where(x => x.CreatedById == request.creatorId).ToListAsync();

                if (todos is null || !todos.Any())
                {
                    return new NotFound();
                }

                var entities = todos.Select(x => x.Adapt<TodoEntity>()).ToList();

                return entities;
            }
            catch (Exception ex)
            {
                return ApplicationError.ApplicationError_Exception(nameof(GetTodosByCreator), ex);
            }
        }
    }
}
