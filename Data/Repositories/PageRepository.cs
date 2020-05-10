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
                        .Include(p => p.CreatedBy)
                        .FirstOrDefault(p => p.CompanyName == companyName);
            return page;
        }

        public Page GetByUrl(string pageUrl)
        {
            var page = appDbContext.Pages
                        .Include(p => p.Testimonials)
                        .Include(p => p.CreatedBy)
                        .FirstOrDefault(p => p.PageURL == pageUrl);
            return page;
        }
    }
}
