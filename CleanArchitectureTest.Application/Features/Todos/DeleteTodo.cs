using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace CleanArchitectureTest.Application.Features.Todos;

public class DeleteTodo
{
    public record Command(Guid id) : IRequest<OneOf<Success, NotFound, ApplicationError>>;

    internal sealed class Handler : IRequestHandler<Command, OneOf<Success, NotFound, ApplicationError>>
    {
        private readonly ApplicationContext _context;

        public Handler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<OneOf<Success, NotFound, ApplicationError>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var todo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);
                if (todo is null)
                {
                    return new NotFound();
                }

                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();

                return new Success();
            }
            catch (Exception ex)
            {
                return ApplicationError.ApplicationError_Exception(nameof(DeleteTodo), ex);
            }
        }
    }
}
