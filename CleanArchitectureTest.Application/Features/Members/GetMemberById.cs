using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Data;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace CleanArchitectureTest.Application.Features.Members;

public class GetMemberById
{
    public record Query(Guid id) : IRequest<OneOf<MemberEntity, NotFound, ApplicationError>>;

    internal sealed class Handler : IRequestHandler<Query, OneOf<MemberEntity, NotFound, ApplicationError>>
    {
        private readonly ApplicationContext _context;

        public Handler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<OneOf<MemberEntity, NotFound, ApplicationError>> Handle(Query request, CancellationToken cancellationToken)
        {
            try
            {
                var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);

                if (member is null)
                {
                    return new NotFound();
                }

                var entity = member.Adapt<MemberEntity>();

                return entity;
            }
            catch (Exception ex)
            {
                return ApplicationError.ApplicationError_Exception(nameof(GetMemberById), ex);
            }
        }
    }
}
