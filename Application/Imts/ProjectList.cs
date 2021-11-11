using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain.Imts;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.Imts
{
    public class ProjectList
    {
        public class Query : IRequest<Result<List<Project>>>
        {

        }

        public class Handler : IRequestHandler<Query, Result<List<Project>>>
        {
            private readonly ImtsContext _context;
            public Handler(ImtsContext context)
            {
                _context = context;


            }
            public async Task<Result<List<Project>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var result = await _context.Projects.Include("office").Where(q => q.office.id == 1).ToListAsync();
                
                return Result<List<Project>>.Success(result);
            }
        }
    }
}