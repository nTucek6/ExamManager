using DatabaseContext;
using Entities;
using Entities.Enum;
using Microsoft.EntityFrameworkCore;

namespace Services.AdminActions
{
    public class AdminActionsService : IAdminActionsService
    {
        private readonly ExamManagerContext database;

        public AdminActionsService(ExamManagerContext database)
        {
            this.database = database;
        }

        public async Task<List<SelectUserRoleDTO>> GetUserRoles()
        {
            var genres = Enum.GetValues(typeof(UserRoleEnum)).Cast<UserRoleEnum>().ToList().Where(q=> q != UserRoleEnum.Admin).Select(x => new SelectUserRoleDTO { value = x, label = x.ToString() }).OrderBy(o => o.label).ToList();
            return genres;
        }

        public async Task<List<UsersEntity>> GetUsers()
        {
            var users = await database.Users.Where(q=> q.RoleId != UserRoleEnum.Admin)
                .Select(s=> new UsersEntity 
                            { Id = s.Id, 
                              Email = s.Email,
                              Name = s.Name,
                              Surname = s.Surname,
                              RoleId = s.RoleId,
                            }).ToListAsync();
            return users;
        }


        public async Task ChangeUserRole(UpdateUserDTO userDTO)
        {
            var user = await database.Users.Where(q=> q.Id == userDTO.UserId).FirstOrDefaultAsync();

            if (user != null)
            {
                user.RoleId = userDTO.UserRole;
            }
           await database.SaveChangesAsync();
        }


    }
}
