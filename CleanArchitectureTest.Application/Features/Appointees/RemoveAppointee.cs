using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Contracts.Responses;
using CleanArchitectureTest.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace CleanArchitectureTest.Application.Features.Appointees;

public class RemoveAppointee
{
    public record Command(Guid todoId, Guid memberId) : IRequest<OneOf<Success, AppointeeNotFound, ApplicationError>>;

    internal sealed class Handler : IRequestHandler<Command, OneOf<Success, AppointeeNotFound, ApplicationError>>
    {
        private readonly ApplicationContext _context;

        public Handler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<OneOf<Success, AppointeeNotFound, ApplicationError>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var appointee = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.memberId, cancellationToken);
                if (appointee is null)
                {
                    return new AppointeeNotFound("Member not found!");
                }

                var ta = await _context.TodoAppointees.FirstOrDefaultAsync(x => x.TodoId == request.todoId
                   && x.AppointeeId == request.memberId, cancellationToken);
                if (ta is null)
                {
                    return new AppointeeNotFound("TodoAppointee not found!");
                }

                _context.Remove(ta);

                await _context.SaveChangesAsync(cancellationToken);

                return new Success();
            }
            catch (Exception ex)
            {
                return ApplicationError.ApplicationError_Exception(nameof(RemoveAppointee), ex);
            }
        }
    }
}
