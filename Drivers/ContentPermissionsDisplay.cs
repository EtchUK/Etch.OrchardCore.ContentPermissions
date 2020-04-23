using Etch.OrchardCore.ContentPermissions.Models;
using Etch.OrchardCore.ContentPermissions.Services;
using Etch.OrchardCore.ContentPermissions.ViewModels;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Security.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.ContentPermissions.Drivers
{
    public class ContentPermissionsDisplay : ContentPartDisplayDriver<ContentPermissionsPart>
    {
        #region Dependencies

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;
        private readonly IContentPermissionsService _contentPermissionsService;

        #endregion

        #region Constructor

        public ContentPermissionsDisplay(IContentDefinitionManager contentDefinitionManager, IHttpContextAccessor httpContextAccessor, IRoleService roleService, IContentPermissionsService contentPermissionsService)
        {
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
            _contentPermissionsService = contentPermissionsService;
        }

        #endregion

        #region Overrides

        public override IDisplayResult Display(ContentPermissionsPart part, BuildPartDisplayContext context)
        {
            if (context.DisplayType != "Detail" || !part.Enabled || _contentPermissionsService.CanAccess(part))
            {
                return null;
            }

            var settings = _contentPermissionsService.GetSettings(part);
            var redirectUrl = settings.HasRedirectUrl ? settings.RedirectUrl : "/Error/403";

            if (!redirectUrl.StartsWith("/"))
            {
                redirectUrl = $"/{redirectUrl}";
            }

            _httpContextAccessor.HttpContext.Response.StatusCode = 403;
            _httpContextAccessor.HttpContext.Response.Redirect($"{_httpContextAccessor.HttpContext.Request.PathBase}{redirectUrl}", false);
            return null;
        }

        public override async Task<IDisplayResult> EditAsync(ContentPermissionsPart part, BuildPartEditorContext context)
        {
            var roles = await _roleService.GetRoleNamesAsync();

            return Initialize<ContentPermissionsPartEditViewModel>("ContentPermissionsPart_Edit", model =>
            {
                model.ContentPermissionsPart = part;
                model.Enabled = part.Enabled;
                model.PossibleRoles = roles.ToArray();
                model.Roles = part.Roles;
            })
            .Location("Parts#Security:10");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentPermissionsPart model, IUpdateModel updater, UpdatePartEditorContext context)
        {
            await updater.TryUpdateModelAsync(model, Prefix, m => m.Enabled, m => m.Roles);

            if (!model.Enabled)
            {
                model.Roles = Array.Empty<string>();
            }

            return Edit(model, context);
        }

        #endregion
    }
}
