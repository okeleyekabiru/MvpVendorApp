using System.ComponentModel.DataAnnotations;

namespace MvpVendingMachineApp.Common
{
    public enum ApiResultStatusCode
    {
        [Display(Name = "OK")]
        Success = 0,

        [Display(Name = "Internal server error")]
        ServerError = 1,

        [Display(Name = "Bad request")]
        BadRequest = 2,

        [Display(Name = "Not found")]
        NotFound = 3,

        [Display(Name = "No Content")]
        NoContent = 4,

        [Display(Name = "UnAuthorized")]
        UnAuthorized = 6
    }
}
