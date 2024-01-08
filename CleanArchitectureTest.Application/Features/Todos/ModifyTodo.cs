using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Data;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace CleanArchitectureTest.Application.Features.Todos
{
    public class ModifyTodo
    {
        public record Command(Guid id, string title, string? description, DateTime? dueDate) : IRequest<OneOf<TodoEntity, NotFound, ApplicationError>>;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.id).NotEmpty();

                RuleFor(x => x.title).NotEmpty();
            }
        }

        internal sealed class Handler : IRequestHandler<Command, OneOf<TodoEntity, NotFound, ApplicationError>>
        {
            private readonly ApplicationContext _context;
            private readonly IValidator<Command> _validator;

            public Handler(ApplicationContext context, IValidator<Command> validator)
            {
                _context = context;
                _validator = validator;
            }

            public async Task<OneOf<TodoEntity, NotFound, ApplicationError>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var validation = _validator.Validate(request);
                    if (!validation.IsValid)
                    {
                        return ApplicationError.ApplicationError_Validation(nameof(ModifyTodo), validation);
                    }

                    //Laden
                    var todo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);
                    if (todo is null)
                    {
                        return new NotFound();
                    }

                    todo.Title = request.title;

                    if (request.description != null && !string.IsNullOrEmpty(request.description))
                    {
                        todo.Description = request.description;
                    }

                    if (request.dueDate is not null)
                    {
                        todo.DueDate = request.dueDate;
                    }

                    todo.UpdatedOn = DateTime.UtcNow;

                    _context.Update(todo);
                    await _context.SaveChangesAsync();

                    var t = todo.Adapt<TodoEntity>();
                    return t;
                }
                catch (Exception ex)
                {
                    return new ApplicationError(nameof(ModifyTodo), "Error", ex.Message);
                }
            }
        }
    }
}
