using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Revoow.Data.IRepositories;

namespace Revoow.Areas.Identity.Pages.Account.Manage
{
    public class UserPagesModel : PageModel
    {
        private readonly IPageRepository _pageRepository;
        private readonly UserManager<RevoowUser> _userManager;

        public UserPagesModel(IPageRepository pageRepository,
                           UserManager<RevoowUser> userManager)
        {
            _pageRepository = pageRepository;
            _userManager = userManager;
        }

        public List<Revoow.Models.Page> RevoowPages { get; set; }

        private async Task LoadAsync(RevoowUser user)
        {
            RevoowPages = _pageRepository.GetAll().Where(p => p.CreatedBy == user).ToList();
        }


        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }
    }
}
