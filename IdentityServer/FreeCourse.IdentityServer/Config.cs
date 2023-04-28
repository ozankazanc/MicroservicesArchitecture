// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {
        //audience'a karsilik gelecek.
        //kaç api server var ise karşılığında o kadar apiResource olmalı.
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes = {"catalog_fullpermission"}},
            new ApiResource("resource_photo_stock"){Scopes = {"photo_stock_fullpermission"}},
            new ApiResource("resource_basket"){Scopes = {"basket_fullpermission"}},
            new ApiResource("resource_discount"){Scopes = {"discount_fullpermission"}},


            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                        new IdentityResources.OpenId(), // mutlaka bu listeye eklenmelidir.
                        new IdentityResources.Email(),
                        new IdentityResources.Profile(),
                        new IdentityResource(){Name = "roles", DisplayName= "Roles",Description = "Kullanıcı Rolleri", UserClaims = new string []{"role"} }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullpermission","Catalog API full erişim"),
                new ApiScope("photo_stock_fullpermission","Photo Stock API full erişim"),
                new ApiScope("basket_fullpermission","Basket API için full erişim"),
                new ApiScope("discount_fullpermission","Discount API için full erişim"),

                new ApiScope(IdentityServerConstants.LocalApi.ScopeName) //IdentityServer için otomatik.
                //new ApiScope("scope1"),
                //new ApiScope("scope2"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                //istersek veritabanında da bu bilgileri tutabiliriz.
                //refresh token'a gerek yok. zaten sadece client id ve client secret kullanıyoruz ve hep sabit.
                new Client
                {
                    ClientName = "Asp.NET.Core MVC",
                    ClientId = "WebMVCClient",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //refresh token yoktur.
                    AllowedScopes = { "catalog_fullpermission", "photo_stock_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
                },
                new Client
                {
                    ClientName = "Asp.NET.Core MVC",
                    ClientId = "WebMVCClientForUser",
                    AllowOfflineAccess = true,
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = {
                        IdentityServerConstants.LocalApi.ScopeName,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "roles",
                        "basket_fullpermission",
                        "discount_fullpermission"
                    },
                    AccessTokenLifetime = 1*60*60, // 1 saat
                    RefreshTokenExpiration = TokenExpiration.Absolute, // sabit tarih vermek için absolute
                    AbsoluteRefreshTokenLifetime = (int) (DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds, // 60 gün
                    RefreshTokenUsage = TokenUsage.ReUse
                }

                #region kapatıldı.
                //// m2m client credentials flow client
                //new Client
                //{
                //    ClientId = "m2m.client",
                //    ClientName = "Client Credentials Client",

                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                //    AllowedScopes = { "scope1" }
                //},

                //// interactive client using code flow + pkce
                //new Client
                //{
                //    ClientId = "interactive",
                //    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                //    AllowedGrantTypes = GrantTypes.Code,

                //    RedirectUris = { "https://localhost:44300/signin-oidc" },
                //    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                //    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                //    AllowOfflineAccess = true,
                //    AllowedScopes = { "openid", "profile", "scope2" }
                //},
                #endregion
            };
    }



}
