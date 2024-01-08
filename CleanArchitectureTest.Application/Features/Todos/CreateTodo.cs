using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Data;
using CleanArchitectureTest.Data.DTOs;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace CleanArchitectureTest.Application.Features.Todos;

public class CreateTodo
{
    public record Command(string title, string? description, DateTime? dueDate, Guid createdBy) :
        IRequest<OneOf<TodoEntity, NotFound, ApplicationError>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.title).NotEmpty();

            RuleFor(x => x.createdBy).NotEmpty();
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
                    return ApplicationError.ApplicationError_Validation(nameof(CreateTodo), validation);
                }

                var creator = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.createdBy, cancellationToken);
                if (creator is null)
                {
                    return new NotFound();
                }

                var todo = new Todo
                {
                    Id = Guid.NewGuid(),
                    Title = request.title,
                    CreatedById = request.createdBy,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                };

                if (request.description is not null)
                {
                    todo.Description = request.description;
                }

                if (request.dueDate is not null)
                {
                    todo.DueDate = request.dueDate;
                }

                _context.Todos.Add(todo);
                await _context.SaveChangesAsync(cancellationToken);

                var t = todo.Adapt<TodoEntity>();
                return t;
            }
            catch (Exception ex)
            {
                return ApplicationError.ApplicationError_Exception(nameof(CreateTodo), ex);
            }
        }
    }
}
