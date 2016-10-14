using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RMS.RequestModel;
using RMS.RequestModel.Auth;
using RMS.RESTfulApi.Models;
using RMS.ViewModel.Auth;

namespace RMS.RESTfulApi.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly UserManager<IdentityUser> _userManager;
        public UserController()
        {
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new RmsAuthContext()));
        }

        [Route("list")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserList()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersVm = new List<UserViewModel>();

            if (users == null)
                return InternalServerError();

            users.ForEach(user => usersVm.Add(new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = _userManager.GetRoles(user.Id).FirstOrDefault()
            }));
            
            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "List of Users",
                Data = usersVm
            });
        }

        [Route("detail/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> FindUser([FromUri]string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "User Detail",
                Data = new UserViewModel
                {
                    UserName = user.UserName,
                    Role = _userManager.GetRoles(user.Id).FirstOrDefault()
                }
            });
        }

        [Route("register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(UserRegistrationRequestModel userInfo)
        {
            if (userInfo == null)
                return BadRequest();

            var user = new IdentityUser
            {
                UserName = userInfo.UserName
            };

            var userSaveResult = await _userManager.CreateAsync(user, userInfo.Password);

            var userDetail = await _userManager.FindByNameAsync(user.UserName);

            var userRoleSaveResult = await _userManager.AddToRoleAsync(userDetail.Id, userInfo.RoleName);

            if (!userSaveResult.Succeeded && !userRoleSaveResult.Succeeded)
                return InternalServerError();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "User Registration Success"
            });
        }

        [Route("role/add")]
        [HttpPost]
        public async Task<IHttpActionResult> AssignUserToRole([FromBody] UserRoleRequestModel userRoleRequestModel)
        {
            var role = await new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new RmsAuthContext())).FindByIdAsync(userRoleRequestModel.RoleId);

            if (role == null)
                return BadRequest();

            var result = await _userManager.AddToRoleAsync(userRoleRequestModel.UserId, role.Name);

            if (!result.Succeeded)
                return InternalServerError();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Role Assigned To User"
            });
        }

        [Route("delete/{userId}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteUser([FromUri]string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
           
            var roles = await _userManager.GetRolesAsync(user.Id);
            var userRoleDeleteResponse = await _userManager.RemoveFromRoleAsync(user.Id, roles.FirstOrDefault());
            var userDeleteResponse = await _userManager.DeleteAsync(user);

            if (!userDeleteResponse.Succeeded && !userRoleDeleteResponse.Succeeded)
                return InternalServerError();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "User Deleted"
            });
        }

    }
}
