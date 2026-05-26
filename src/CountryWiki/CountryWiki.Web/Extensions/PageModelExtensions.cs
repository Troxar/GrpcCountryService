namespace CountryWiki.Web.Extensions;

public static class PageModelExtensions
{
    extension(PageModel pageModel)
    {
        public IActionResult ToActionResult(CountryServiceException exception)
        {
            return exception.ErrorCode switch
            {
                CountryServiceErrorCode.NotFound => pageModel.NotFound(),
                _ => pageModel.RedirectToPage("/Error", new { message = exception.Message })
            };
        }
    }
}