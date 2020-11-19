

using System;
using Microsoft.AspNetCore.Identity;

namespace aspnetdemo2 {

    public static class SeedIdentityUsers {
        public static void SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            CrearRoles(roleManager);
            CrearUsuarios(userManager);

        }

        private static void CrearRoles(RoleManager<IdentityRole> roleManager)
        {
            if(!roleManager.RoleExistsAsync("Estudiante").GetAwaiter().GetResult()){
                var estudianteRole = new IdentityRole();
                estudianteRole.Name = "Estudiante";
                var res = roleManager.CreateAsync(estudianteRole).GetAwaiter().GetResult();
            }
            if(!roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult()){
                var adminRole = new IdentityRole();
                adminRole.Name = "Admin";
                var res = roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
            }
        }

        private static void CrearUsuarios(UserManager<IdentityUser> userManager)
        {
            if(userManager.FindByEmailAsync("admin@localhost").GetAwaiter().GetResult() == null){
                var admin = new IdentityUser();
                admin.UserName = "admin@localhost";
                admin.Email = "admin@localhost";
                admin.EmailConfirmed = true;

                var res = userManager.CreateAsync(admin,
                            "123456")
                            .GetAwaiter().GetResult();
                    
                if(res.Succeeded){
                    var resRole  = userManager.AddToRoleAsync(admin, "Admin")
                        .GetAwaiter().GetResult();
                }

            }
        }
    }

}