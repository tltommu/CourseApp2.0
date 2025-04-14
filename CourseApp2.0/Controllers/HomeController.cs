using CourseApp2._0.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth;

namespace CourseApp2._0.Controllers
{
    public class HomeController : Controller
    {
      
        public IActionResult Index()
        {
            return View();
        }

        
        [Authorize]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Logout action.
        /// Does nothing if the user is not logged in.
        /// </summary>
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return View();
        }

       
        [Authorize]
        public async Task<IActionResult> ScopeListing([FromServices] IGoogleAuthProvider auth)
        {
            IReadOnlyList<string> currentScopes = await auth.GetCurrentScopesAsync();
            return View(currentScopes);
        }

     
        [GoogleScopedAuthorize(DriveService.ScopeConstants.DriveReadonly)]
        public async Task<IActionResult> DriveFileList([FromServices] IGoogleAuthProvider auth)
        {
            GoogleCredential cred = await auth.GetCredentialAsync();
            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = cred
            });
            var files = await service.Files.List().ExecuteAsync();
            var fileNames = files.Files.Select(x => x.Name).ToList();
            return View(fileNames);
        }

        

        public IActionResult Privacy()
        {
            return View();
        }


    
        [Authorize]
        public async Task<IActionResult> ShowTokens()
        {
            // The user is already authenticated, so this call won't trigger authentication.
            // But it allows us to access the AuthenticateResult object that we can inspect
            // to obtain token related values.
            AuthenticateResult auth = await HttpContext.AuthenticateAsync();
            string idToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.IdToken);
            string idTokenValid, idTokenIssued, idTokenExpires;
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                idTokenValid = "true";
                idTokenIssued = new DateTime(1970, 1, 1).AddSeconds(payload.IssuedAtTimeSeconds.Value).ToString();
                idTokenExpires = new DateTime(1970, 1, 1).AddSeconds(payload.ExpirationTimeSeconds.Value).ToString();
            }
            catch (Exception e)
            {
                idTokenValid = $"false: {e.Message}";
                idTokenIssued = "invalid";
                idTokenExpires = "invalid";
            }
            string accessToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.AccessToken);
            string refreshToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.RefreshToken);
            string accessTokenExpiresAt = auth.Properties.GetTokenValue("expires_at");
            string cookieIssuedUtc = auth.Properties.IssuedUtc?.ToString() ?? "<missing>";
            string cookieExpiresUtc = auth.Properties.ExpiresUtc?.ToString() ?? "<missing>";

            return View(new[]
            {
                $"Id Token: '{idToken}'",
                $"Id Token valid: {idTokenValid}",
                $"Id Token Issued UTC: '{idTokenIssued}'",
                $"Id Token Expires UTC: '{idTokenExpires}'",
                $"Access Token: '{accessToken}'",
                $"Refresh Token: '{refreshToken}'",
                $"Access token expires at: '{accessTokenExpiresAt}'",
                $"Cookie Issued UTC: '{cookieIssuedUtc}'",
                $"Cookie Expires UTC: '{cookieExpiresUtc}'",
            });
        }

        public class ForceTokenRefreshModel
        {
            public IReadOnlyList<string> Results;
            public string AccessToken;
        }

        
        [Authorize]
        public async Task<IActionResult> ForceTokenRefresh([FromServices] IGoogleAuthProvider auth)
        {
            // Obtain OAuth related values before the refresh.
            AuthenticateResult authResult0 = await HttpContext.AuthenticateAsync();
            string accessToken0 = authResult0.Properties.GetTokenValue(OpenIdConnectParameterNames.AccessToken);
            string refreshToken0 = authResult0.Properties.GetTokenValue(OpenIdConnectParameterNames.RefreshToken);
            string issuedUtc0 = authResult0.Properties.IssuedUtc?.ToString() ?? "<missing>";
            string expiresUtc0 = authResult0.Properties.ExpiresUtc?.ToString() ?? "<missing>";

            // Force token refresh by specifying a too-long refresh window.
            GoogleCredential cred = await auth.GetCredentialAsync(TimeSpan.FromHours(24));

            // Obtain OAuth related values after the refresh.
            AuthenticateResult authResult1 = await HttpContext.AuthenticateAsync();
            string accessToken1 = authResult1.Properties.GetTokenValue(OpenIdConnectParameterNames.AccessToken);
            string refreshToken1 = authResult1.Properties.GetTokenValue(OpenIdConnectParameterNames.RefreshToken);
            string issuedUtc1 = authResult1.Properties.IssuedUtc?.ToString() ?? "<missing>";
            string expiresUtc1 = authResult1.Properties.ExpiresUtc?.ToString() ?? "<missing>";

            // As demonstration compare the old values with the new ones and check that everything is
            // as it should be.
            string credAccessToken = await cred.UnderlyingCredential.GetAccessTokenForRequestAsync();

            bool accessTokenChanged = accessToken0 != accessToken1;
            bool credHasCorrectAccessToken = credAccessToken == accessToken1;

            bool pass = accessTokenChanged && credHasCorrectAccessToken;

            var model = new ForceTokenRefreshModel
            {
                Results = new[]
                {
                    $"Before Access Token: '{accessToken0}'",
                    $"Before Refresh Token: '{refreshToken0}'",
                    $"Before Issued UTC: '{issuedUtc0}'",
                    $"Before Expires UTC: '{expiresUtc0}'",
                    $"After Access Token: '{accessToken1}'",
                    $"After Refresh Token: '{refreshToken1}'",
                    $"After Issued UTC: '{issuedUtc1}'",
                    $"After Expires UTC: '{expiresUtc1}'",
                    $"Access token changed: {accessTokenChanged}",
                    $"Credential has correct access token: {credHasCorrectAccessToken}",
                    $"Pass: {pass}"
                },
                AccessToken = accessToken1
            };
            return View(model);
        }

       
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ForceTokenRefreshCheckPersisted([FromServices] IGoogleAuthProvider auth, [FromForm] string expectedAccessToken)
        {
            // Check that the refreshed access token is correctly persisted across requests.
            var cred = await auth.GetCredentialAsync();
            var credAccessToken = await cred.UnderlyingCredential.GetAccessTokenForRequestAsync();
            var pass = credAccessToken == expectedAccessToken;
            return View(new[]
            {
                $"Expected access token: '{expectedAccessToken}'",
                $"Credential access token: '{credAccessToken}'",
                $"Pass: {pass}"
            });
        }
    }
}