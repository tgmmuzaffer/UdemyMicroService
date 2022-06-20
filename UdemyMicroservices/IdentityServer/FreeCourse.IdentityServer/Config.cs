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
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("resource_catalog"){Scopes={ "catalog_fullpermission"} },
                new ApiResource("resource_photo_stock"){Scopes={ "photo_stock_fullpermission" } },
                new ApiResource("resource_basket"){Scopes={ "basket_fullpermission" } },
                new ApiResource("resource_discount"){Scopes={ "discount_fullpermission" } },
                new ApiResource("resource_order"){Scopes={ "order_fullpermission" } },
                new ApiResource("resource_fake_payment"){Scopes={ "fake_payment_fullpermission" } },
                new ApiResource("resource_gateway"){Scopes={ "gateway_fullpermission" } },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource{Name="roles", DisplayName="Roles", Description="Kullanıcı rolleri", UserClaims=new []{"role"} }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullpermission", "CatalogApi için tam yetkili erişim."),
                new ApiScope("photo_stock_fullpermission", "Photo Stock Api için tam yetkili erişim."),
                new ApiScope("basket_fullpermission", "Basket Api için tam yetkili erişim."),
                new ApiScope("discount_fullpermission", "Discount Api için tam yetkili erişim."),
                new ApiScope("order_fullpermission", "Order Api için tam yetkili erişim."),
                new ApiScope("fake_payment_fullpermission", "Fake_payment Api için tam yetkili erişim."),
                new ApiScope("gateway_fullpermission", "Gateway Api için tam yetkili erişim."),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)

            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
               new Client
               {
                   ClientName ="Asp.Net Core Mvc",
                   ClientId="WebMvcClient",
                   ClientSecrets ={ new Secret("secret".Sha256()) },
                   AllowedGrantTypes = GrantTypes.ClientCredentials,//sabit bir clientıd ve clientsecret olduğundan burada refresh token olmaz. 
                   AllowedScopes = { "gateway_fullpermission", "catalog_fullpermission" , "photo_stock_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
               },
                new Client
               {
                   ClientName ="Asp.Net Core Mvc",
                   ClientId="WebMvBcClientForUser",
                   ClientSecrets ={ new Secret("secret".Sha256()) },
                   AllowOfflineAccess= true,
                   AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // refresh token alabilmek için akış tıpini bu seçtik
                   AllowedScopes = { "fake_payment_fullpermission", "order_fullpermission", "basket_fullpermission", "discount_fullpermission", IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess, IdentityServerConstants.LocalApi.ScopeName, "roles" },//offline => refresh token ı kullanıcı online olmadığı durumda identity serverdan yerni token alabildiği için ismi oflineaccess
                   AccessTokenLifetime = 1*60*60,
                   RefreshTokenExpiration = TokenExpiration.Absolute,
                   AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                   RefreshTokenUsage = TokenUsage.ReUse
               }
                };
    }
}