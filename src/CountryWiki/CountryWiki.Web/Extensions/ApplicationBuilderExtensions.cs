namespace CountryWiki.Web.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCountryWikiPipeline(this IApplicationBuilder builder, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            builder.UseDeveloperExceptionPage();
        }
        else
        {
            builder.UseExceptionHandler("/Error");
            builder.UseHsts();
            builder.UseHttpsRedirection();
        }

        builder.UseStaticFiles();
        builder.UseRouting();
        builder.UseAuthorization();

        return builder;
    }
}