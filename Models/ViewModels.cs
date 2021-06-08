using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Tider.Models {

    public class EditApplicationUserViewModel {
        public string NickName { get; set; }
        public string RealName { get; set; }
        public string Image_url { get; set; }
    }

    public class ProfilePageViewModel {
        public ApplicationUser User { get; set; }
        public ICollection<Post> Posts { get; set; }
        public int PostsCount { get; set; }
    }

    public class PostsViewModel {
        public bool IsAdmin { get; set; }
        public bool IsMod { get; set; }
        public bool IsUser { get; set; }
        public string UserImage { get; set; }
        public ICollection<Post> Posts { get; set; }
        public int PostsCount { get; set; }
        public int? CategoryID { get; set; }
    }

    public class CommentsViewModel {
        public bool IsAdmin { get; set; }
        public bool IsMod { get; set; }
        public bool IsUser { get; set; }
        public string UserImage { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public Post Post { get; set; }
        public int CommentsCount { get; set; }
        public int? PostId { get; set; }
    }

    public class ProfilesTableViewModel {
        public ICollection<Tuple<ApplicationUser, string>> PostRoleTuples { get; set; }
    }


    // ACCOUNT VIEW MODELS

    public class ExternalLoginConfirmationViewModel {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel {
        public string EmailPlaceholder { get; set; }
        public string PasswordPlaceholder { get; set; }
        public bool InvalidLogin { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel {
        [Required]
        public string NickName { get; set; }
        public bool IsOkNickName { get; set; }
        public bool IsOkEmail { get; set; }
        public bool IsOkPassword { get; set; }
        public bool IsOkPassword2 { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}