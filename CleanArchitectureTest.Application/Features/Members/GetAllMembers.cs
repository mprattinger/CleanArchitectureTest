using CleanArchitectureTest.Contracts;
using CleanArchitectureTest.Contracts.Entities;
using CleanArchitectureTest.Data;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CleanArchitectureTest.Application.Features.Members;

public class GetAllMembers
{
    public record Query() : IRequest<OneOf<List<MemberEntity>, ApplicationError>>;

    internal sealed class Handler : IRequestHandler<Query, OneOf<List<MemberEntity>, ApplicationError>>
    {
        private readonly ApplicationContext _context;

        public Handler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<OneOf<List<MemberEntity>, ApplicationError>> Handle(Query request, CancellationToken cancellationToken)
        {
            try
            {
                var members = await _context.Members.ToListAsync(cancellationToken);

                return members.Select(m => m.Adapt<MemberEntity>()).ToList();
            }
            catch (Exception ex)
            {
                return ApplicationError.ApplicationError_Exception(nameof(GetAllMembers), ex);
            }
        }
    }
}
