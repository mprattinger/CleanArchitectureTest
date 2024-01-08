using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Data;
using CleanArchitectureTest.Data.DTOs;
using FluentValidation;
using Mapster;
using MediatR;
using OneOf;

namespace CleanArchitectureTest.Application.Features.Members;

public class CreateMember
{
    public record Command(string firstName, string lastName) : IRequest<OneOf<MemberEntity, ApplicationError>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.firstName).NotEmpty();

            RuleFor(x => x.lastName).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, OneOf<MemberEntity, ApplicationError>>
    {
        private readonly ApplicationContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<OneOf<MemberEntity, ApplicationError>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var validation = _validator.Validate(request);
                if (!validation.IsValid)
                {
                    return ApplicationError.ApplicationError_Validation(nameof(CreateMember), validation);
                }

                var member = new Member
                {
                    Id = Guid.NewGuid(),
                    FirstName = request.firstName,
                    LastName = request.lastName,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                };

                _context.Members.Add(member);
                await _context.SaveChangesAsync();

                var m = member.Adapt<MemberEntity>();
                return m;
            }
            catch (Exception ex)
            {
                return ApplicationError.ApplicationError_Exception(nameof(CreateMember), ex);
            }
        }
    }
}
