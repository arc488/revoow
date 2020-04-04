using Revoow.Data.IRepositories;
using Revoow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Data.Repositories
{
    public class TestimonialRepository : Repository<Testimonial>, ITestimonialRepository
    {
        public TestimonialRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

    }
}
