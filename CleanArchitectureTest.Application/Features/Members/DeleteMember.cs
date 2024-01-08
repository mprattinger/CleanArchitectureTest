using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace CleanArchitectureTest.Application.Features.Members;

public class DeleteMember
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
                var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);
                if (member is null)
                {
                    return new NotFound();
                }

                _context.Members.Remove(member);
                await _context.SaveChangesAsync();

                return new Success();
            }
            catch (Exception ex)
            {
                return ApplicationError.ApplicationError_Exception(nameof(ModifyMember), ex);
            }
        }
    }
}