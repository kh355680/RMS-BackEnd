using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using RMS.BusinessModel.Context;
using RMS.BusinessModel.Entities;
using RMS.BusinessService.Services;
using RMS.Repository.Repositories;
using RMS.ViewModel;

namespace RMS.RESTfulApi.Controllers
{
    [RoutePrefix("api/food/category")]
    public class FoodCategoryController : ApiController
    {
        private readonly FoodCategoryService _service;
        private readonly ResponseModel _responseModel;

        public FoodCategoryController()
        {
            _service = new FoodCategoryService(new FoodCategoryRepository(new RmsDbContext()));
            _responseModel = new ResponseModel();
        }

        [Route("list")]
        [HttpGet]
        public async Task<IHttpActionResult> GetList()
        {
            var foodCategories = await _service.Get().AsQueryable().ToListAsync();

            if (foodCategories == null)
                return NotFound();

            _responseModel.IsSuccess = true;
            _responseModel.Message = "List Return OK";
            _responseModel.Data = foodCategories.Select(item => new FoodCategoryViewModel
            {
                Id = item.Id,
                Name = item.Name
            });

            return Ok(_responseModel);
        }

        [Route("detail/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri]string id)
        {
            var item = _service.Get(id);


            if (item == null)
                return NotFound();

            _responseModel.IsSuccess = true;
            _responseModel.Message = "Category Return OK";
            _responseModel.Data = new FoodCategoryViewModel
            {
                Id = item.Id,
                Name = item.Name
            };

            return Ok(_responseModel);
        }

        [Route("add")]
        [HttpPost]
        public async Task<IHttpActionResult> Add([FromBody]FoodCategoryViewModel foodCategoryViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _service.Insert(new FoodCategory
            {
                Name = foodCategoryViewModel.Name
            });

            _responseModel.IsSuccess = true;
            _responseModel.Message = $"{result.Name} Food Cateogory Added Successfully";
            _responseModel.Data = new FoodCategoryViewModel
            {
                Id = result.Id,
                Name = result.Name
            };

            return Ok(_responseModel);
        }

        [Route("update/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> Update([FromBody]FoodCategoryViewModel foodCategoryViewModel, [FromUri]string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _service.Update(new FoodCategory
            {
                Id = id,
                Name = foodCategoryViewModel.Name
            });

            if (result == null)
                return InternalServerError();

            _responseModel.IsSuccess = true;
            _responseModel.Message = $"{result.Name} updated successfully";
            _responseModel.Data = new FoodCategoryViewModel
            {
                Id = result.Id,
                Name = result.Name
            };
            return Ok(_responseModel);
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete([FromUri]string id)
        {
            var result = _service.Remove(id);

            if (result == null)
                return InternalServerError();

            _responseModel.IsSuccess = true;
            _responseModel.Message = $"{result.Name} Category deleted successfully";

            return Ok(_responseModel);
        }
    }
}
