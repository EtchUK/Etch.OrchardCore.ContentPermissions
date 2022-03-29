using Etch.OrchardCore.ContentPermissions.Models;
using OrchardCore.ContentManagement;
using System;
using YesSql.Indexes;

namespace Etch.OrchardCore.ContentPermissions.Indexing
{
    public class ContentPermissionsPartIndex : MapIndex
    {
        public bool Enabled { get; set; }
        public string[] Roles { get; set; }
    }

    public class ContentPermissionsPartIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<ContentPermissionsPartIndex>()
                .Map(contentItem =>
                {
                    var part = contentItem.As<ContentPermissionsPart>();

                    if (part == null)
                    {
                        return null;
                    }

                    return new ContentPermissionsPartIndex
                    {
                        Enabled = part.Enabled,
                        Roles = part.Roles
                    };
                });
        }
    }
}
