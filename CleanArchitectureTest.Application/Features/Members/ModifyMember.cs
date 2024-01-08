using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Data;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace CleanArchitectureTest.Application.Features.Members;

public class ModifyMember
{
    public record Command(Guid id, string firstName, string lastName) : IRequest<OneOf<MemberEntity, NotFound, ApplicationError>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.id).NotEmpty();

            RuleFor(x => x.firstName).NotEmpty();

            RuleFor(x => x.lastName).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, OneOf<MemberEntity, NotFound, ApplicationError>>
    {
        private ApplicationContext _context;
        private IValidator<Command> _validator;

        public Handler(ApplicationContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<OneOf<MemberEntity, NotFound, ApplicationError>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var validation = _validator.Validate(request);
                if (!validation.IsValid)
                {
                    return ApplicationError.ApplicationError_Validation(nameof(ModifyMember), validation);
                }

                //Laden
                var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);
                if (member is null)
                {
                    return new NotFound();
                }

                member.FirstName = request.firstName;
                member.LastName = request.lastName;
                member.UpdatedOn = DateTime.UtcNow;

                _context.Update(member);
                await _context.SaveChangesAsync();

                var m = member.Adapt<MemberEntity>();
                return m;
            }
            catch (Exception ex)
            {
                return ApplicationError.ApplicationError_Exception(nameof(ModifyMember), ex);
            }
        }
    }
}
