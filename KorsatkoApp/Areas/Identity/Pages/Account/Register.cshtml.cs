// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using KorsatkoApp.Areas.Admin.Models;
using System.ComponentModel;

namespace KorsatkoApp.Areas.Identity.Pages.Account {
	public class RegisterModel : PageModel {
		private readonly SignInManager<Student> _signInManager;
		private readonly UserManager<Student> _userManager;
		private readonly IUserStore<Student> _userStore;
		private readonly IUserEmailStore<Student> _emailStore;
		private readonly ILogger<RegisterModel> _logger;
		private readonly IEmailSender _emailSender;

		public RegisterModel(
			UserManager<Student> userManager,
			IUserStore<Student> userStore,
			SignInManager<Student> signInManager,
			ILogger<RegisterModel> logger,
			IEmailSender emailSender) {
			_userManager = userManager;
			_userStore = userStore;
			_emailStore = GetEmailStore();
			_signInManager = signInManager;
			_logger = logger;
			_emailSender = emailSender;
		}

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
		public string ReturnUrl { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		public class InputModel {
			[DisplayName("الاسم بالكامل")]
			[Required(ErrorMessage = "يجب ادخال اسم بالكامل")]
			public string FullName { get; set; }


			[DisplayName("اسم المستخدم")]
			[Required(ErrorMessage = "يجب ادخال اسم المستخدم")]
			public string UserName { get; set; }


			[DisplayName("الجنس")]
			[Required(ErrorMessage = "يجب تحديد الجنس")]
			public char Gender { get; set; }
			[Display(Name = "رقم الهاتف")]
			public string PhoneNumber { get; set; }

			[DisplayName("تاريخ الميلاد")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            [Required(ErrorMessage = "يجب ادخال تاريخ الميلاد")]
			public DateTime DateOfBirth { get; set; }


			[DisplayName("الرقم القومي")]
			[Required(ErrorMessage = "يجب ادخال الرقم القومي"), StringLength(14)]
			public string NationalId { get; set; }


			[EmailAddress]
			[Required(ErrorMessage = "يجب ادخال البريد الإلكتروني")]
			[Display(Name = "البريد الإلكتروني")]
			public string Email { get; set; }


			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "كلمة السر")]
			public string Password { get; set; }


			[DataType(DataType.Password)]
			[Display(Name = "تأكيد كلمة السر")]
			[Compare("Password", ErrorMessage = "كلمة السر المدخلة لا تطابق كلمة السر المدخلة سابقا ")]
			public string ConfirmPassword { get; set; }

			public DateTime AddedOn { get; set; } = DateTime.Now;

		}


		public async Task OnGetAsync(string returnUrl = null) {
			ReturnUrl = returnUrl;
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null) {
			returnUrl ??= Url.Content("~/");
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
			if (ModelState.IsValid) {
				var user = CreateUser();
				user.FullName = Input.FullName;
				user.Email = Input.Email;
				user.UserName = Input.Email;
				user.Gender = Input.Gender;
				user.DateOfBirth = Input.DateOfBirth;
				user.NationalId = Input.NationalId;
				user.PhoneNumber = Input.PhoneNumber;
				user.AddedOn = DateTime.Now;
				await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
				await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
				var result = await _userManager.CreateAsync(user, Input.Password);

				if (result.Succeeded) {
					_logger.LogInformation("User created a new account with password.");

					var userId = await _userManager.GetUserIdAsync(user);
					var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
					var callbackUrl = Url.Page(
						"/Account/ConfirmEmail",
						pageHandler: null,
						values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
						protocol: Request.Scheme);

					await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
						$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

					if (_userManager.Options.SignIn.RequireConfirmedAccount) {
						return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
					} else {
						await _signInManager.SignInAsync(user, isPersistent: false);
						return LocalRedirect(returnUrl);
					}
				}
				foreach (var error in result.Errors) {
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}

		private Student CreateUser() {
			try {
				return Activator.CreateInstance<Student>();
			} catch {
				throw new InvalidOperationException($"Can't create an instance of '{nameof(Student)}'. " +
					$"Ensure that '{nameof(Student)}' is not an abstract class and has a parameterless constructor, or alternatively " +
					$"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
			}
		}

		private IUserEmailStore<Student> GetEmailStore() {
			if (!_userManager.SupportsUserEmail) {
				throw new NotSupportedException("The default UI requires a user store with email support.");
			}
			return (IUserEmailStore<Student>)_userStore;
		}
	}
}
