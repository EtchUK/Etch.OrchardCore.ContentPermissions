using Etch.OrchardCore.ContentPermissions.Models;
using Etch.OrchardCore.ContentPermissions.ViewModels;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Security.Services;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Etch.OrchardCore.ContentPermissions.Drivers
{
    public class ContentPermissionsDisplay : ContentPartDisplayDriver<ContentPermissionsPart>
    {
        #region Dependencies

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;

        #endregion

        #region Constructor

        public ContentPermissionsDisplay(IHttpContextAccessor httpContextAccessor, IRoleService roleService)
        {
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
        }

        #endregion

        #region Overrides

        public override IDisplayResult Display(ContentPermissionsPart part, BuildPartDisplayContext context)
        {
            if (!part.Enabled || CanAccess(_httpContextAccessor.HttpContext.User, part.Roles))
            {
                return null;
            }

            _httpContextAccessor.HttpContext.Response.StatusCode = 403;
            _httpContextAccessor.HttpContext.Response.Redirect($"{_httpContextAccessor.HttpContext.Request.PathBase}/Error/403", false);
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

            return Edit(model, context);
        }

        #endregion

        #region Helpers

        private bool CanAccess(ClaimsPrincipal user, string[] roles)
        {
            if (user == null)
            {
                return false;
            }

            foreach (var role in roles)
            {
                if (user.IsInRole(role))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
