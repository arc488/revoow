using Revoow.Data.IRepositories;
using Revoow.Data;
using Revoow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Revoow.Data.Repositories
{
    public class PageRepository : Repository<Page>, IPageRepository
    {
        public PageRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public Page GetByName(string companyName)
        {
            var page = appDbContext.Pages
                        .Include(p => p.Testimonials)
                        .FirstOrDefault(p => p.CompanyName == companyName);   
            return page;
        }
    }
}
