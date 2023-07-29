using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.DataAccess;

public static class UserDataSeeder
{
    public static async Task Seed(IServiceProvider service)
    {

         
        // UserManager (Repositorio de Usuarios)
        var userManager = service.GetRequiredService<UserManager<ShopPhoneUserIdentity>>();

        // RoleManager (Repositorio de Roles)
        var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

        // Crear Roles
        var adminRole = new IdentityRole("Admin");
        var userRole = new IdentityRole("User");

        if (!await roleManager.RoleExistsAsync("User"))
            await roleManager.CreateAsync(userRole);

        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(adminRole);



        var adminUser = new ShopPhoneUserIdentity
        {
            FirstName = "Administrador",
            LastName = "del Sistema",
            DocumentNumber = "12345678",
            Email = "admin@gmail.com",
            UserName = "admin@gmail.com",
            PhoneNumber = "+51 999 000 000",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, "123456*");
        if (result.Succeeded)
        {
            adminUser = await userManager.FindByEmailAsync(adminUser.Email);
            if (adminUser is not null)
                await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}