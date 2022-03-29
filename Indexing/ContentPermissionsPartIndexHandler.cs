using Etch.OrchardCore.ContentPermissions.Models;
using OrchardCore.Indexing;
using System.Threading.Tasks;

namespace Etch.OrchardCore.ContentPermissions.Indexing
{
    public class ContentPermissionsPartIndexHandler : ContentPartIndexHandler<ContentPermissionsPart>
    {
        public override Task BuildIndexAsync(ContentPermissionsPart part, BuildPartIndexContext context)
        {
            context.DocumentIndex.Set($"{nameof(ContentPermissionsPart)}.{nameof(ContentPermissionsPart.Enabled)}", part.Enabled, DocumentIndexOptions.Store);

            foreach (var role in part.Roles)
            {
                context.DocumentIndex.Set($"{nameof(ContentPermissionsPart)}.{nameof(ContentPermissionsPart.Roles)}", role, DocumentIndexOptions.Store);
            }

            return Task.CompletedTask;
        }
    }
}
