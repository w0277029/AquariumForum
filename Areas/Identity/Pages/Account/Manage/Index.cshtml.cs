// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AquariumForum.Data;

namespace AquariumForum.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            ////////////////////////////////////////
            // BEGIN: ApplicationUser custom fields
            ////////////////////////////////////////

            [Required]
            [Display(Name = "Name or Handle")]
            public string Name { get; set; }

            [Display(Name = "Short Bio")]
            public string Bio { get; set; }

            [Display(Name = "Location")]
            public string Location { get; set; }

            [Display(Name = "Available for Hire")]
            public bool IsForHire { get; set; }

            public string ImageFilename { get; set; }

            [Display(Name = "Profile Picture")]
            public IFormFile ImageFile { get; set; }

            ////////////////////////////////////////
            // End: ApplicationUser custom fields
            ////////////////////////////////////////

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                ////////////////////////////////////////
                // BEGIN: ApplicationUser custom fields
                ////////////////////////////////////////
                Name = user.Name,
                Bio = user.Bio,
                Location = user.Location,
                IsForHire = user.IsForHire,
                ImageFilename = user.ImageFilename,
                ////////////////////////////////////////
                // End: ApplicationUser custom fields
                ////////////////////////////////////////
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            ////////////////////////////////////////
            // BEGIN: ApplicationUser custom fields
            ////////////////////////////////////////
            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }

            if (Input.Name != user.Bio)
            {
                user.Bio = Input.Bio;
            }

            if (Input.Location != user.Location)
            {
                user.Location = Input.Location;
            }

            if (Input.IsForHire != user.IsForHire)
            {
                user.IsForHire = Input.IsForHire;
            }

            // Save the uploaded profile picture in db and folder
            if (Input.ImageFile != null)
            {
                string imageFilename = Guid.NewGuid().ToString() + Path.GetExtension(Input.ImageFile?.FileName);

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile_img", imageFilename);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ImageFile.CopyToAsync(fileStream);
                }

                user.ImageFilename = imageFilename; // change the filename
            }

            await _userManager.UpdateAsync(user);

            ////////////////////////////////////////
            // End: ApplicationUser custom fields
            ////////////////////////////////////////

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
