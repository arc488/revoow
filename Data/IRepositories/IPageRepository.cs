using Revoow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Data.IRepositories
{
    public interface IPageRepository : IRepository<Page>
    {

        Page GetByName(string companyName);

    }
}
