var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

//---------> SECURITY HEADERS

var policyCollection = new HeaderPolicyCollection()
        .AddFrameOptionsDeny()
        .AddXssProtectionBlock()
        .AddContentTypeOptionsNoSniff()
        .AddStrictTransportSecurityMaxAgeIncludeSubDomains(maxAgeInSeconds: 60 * 60 * 24 * 365) // maxage = one year in seconds
        .AddReferrerPolicyStrictOriginWhenCrossOrigin()
        .RemoveServerHeader()
        .AddContentSecurityPolicy(builder =>
        {
            builder.AddObjectSrc().None();
            builder.AddFormAction().Self();
            builder.AddFrameAncestors().None();
        })
        .AddCrossOriginOpenerPolicy(builder =>
        {
            builder.SameOrigin();
        })
        .AddCrossOriginEmbedderPolicy(builder =>
        {
            builder.RequireCorp();
        })
        .AddCrossOriginResourcePolicy(builder =>
        {
            builder.SameOrigin();
        })
        .AddCustomHeader("X-My-Test-Header", "Header value");

app.UseSecurityHeaders(policyCollection);

//---------> SECURITY HEADERS


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
