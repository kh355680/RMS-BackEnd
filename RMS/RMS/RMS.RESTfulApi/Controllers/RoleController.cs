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
    [RoutePrefix("api/role")]
    public class RoleController : ApiController
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController()
        {
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new RmsAuthContext()));
        }

        [Route("list")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRoles()
        {
            var rolesVm = await _roleManager.Roles.Select(x => new RoleViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            if (rolesVm == null)
                return NotFound();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Roles List"
            });
        }

        [Route("detail/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRoleById([FromUri]string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                return NotFound();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "User Detail",
                Data = new
                {
                    role.Id,
                    role.Name
                }
            });
        }

        [Route("create")]
        [HttpPost]
        public async Task<IHttpActionResult> Create(RoleRequestModel roleRequestModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var result = await _roleManager.CreateAsync(new IdentityRole
            {
                Name = roleRequestModel.Name
            });

            if (!result.Succeeded)
                return InternalServerError();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Role Created"
            });
        }


        [Route("update/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> Update([FromBody]RoleRequestModel roleRequestModel,[FromUri]string id)
        {
            var roleExist = await _roleManager.FindByIdAsync(id);

            if (roleExist == null)
                return BadRequest();

            roleExist.Name = roleRequestModel.Name;

            var result = await _roleManager.UpdateAsync(roleExist);

            if (!result.Succeeded)
                return InternalServerError();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Role Updated"
            });
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRole([FromUri]string id)
        {
            var roleExist = await _roleManager.FindByIdAsync(id);

            var result = await _roleManager.DeleteAsync(roleExist);

            if (!result.Succeeded)
                return InternalServerError();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Role Deleted"
            });
        }
    }
}

