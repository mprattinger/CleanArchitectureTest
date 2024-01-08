using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Data;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace CleanArchitectureTest.Application.Features.Todos;

public class GetTodoById
{
    public record Query(Guid id) : IRequest<OneOf<TodoEntity, NotFound, ApplicationError>>;

    internal sealed class Handler : IRequestHandler<Query, OneOf<TodoEntity, NotFound, ApplicationError>>
    {
        private readonly ApplicationContext _context;

        public Handler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<OneOf<TodoEntity, NotFound, ApplicationError>> Handle(Query request, CancellationToken cancellationToken)
        {
            try
            {
                var todo = await _context
                    .Todos
                    .FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);

                if (todo is null)
                {
                    return new NotFound();
                }

                var entity = todo.Adapt<TodoEntity>();

                return entity;
            }
            catch (Exception ex)
            {
                return ApplicationError.ApplicationError_Exception(nameof(GetTodoById), ex);
            }
        }
    }
}
