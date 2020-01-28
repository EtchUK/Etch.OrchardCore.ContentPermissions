using Etch.OrchardCore.ContentPermissions.Drivers;
using Etch.OrchardCore.ContentPermissions.Models;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace Etch.OrchardCore.ContentPermissions
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IContentPartDisplayDriver, ContentPermissionsDisplay>();
            services.AddContentPart<ContentPermissionsPart>();

            services.AddScoped<IDataMigration, Migrations>();
        }
    }
}
