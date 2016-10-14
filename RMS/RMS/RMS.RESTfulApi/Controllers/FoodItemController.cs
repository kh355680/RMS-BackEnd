using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using RMS.BusinessModel.Context;
using RMS.BusinessModel.Entities;
using RMS.BusinessService.Services;
using RMS.Repository.Repositories;
using RMS.RequestModel;
using RMS.UtilityService;
using RMS.ViewModel;
using ResponseModel = RMS.ViewModel.ResponseModel;

namespace RMS.RESTfulApi.Controllers
{
    [RoutePrefix("api/food/item")]
    public class FoodItemController : ApiController
    {
        private readonly FoodCategoryService _foodCategoryService;
        private readonly FoodItemService _service;

        public FoodItemController()
        {
            _foodCategoryService = new FoodCategoryService(new FoodCategoryRepository(new RmsDbContext()));
            _service = new FoodItemService(new FoodItemRepository(new RmsDbContext()));
        }

        [Route("list")]
        [HttpGet]
        public async Task<IHttpActionResult> GetList()
        {
            var foodItems = await _service.Get().AsQueryable().ToListAsync();

            if (foodItems == null)
                return NotFound();

            var response = new ResponseModel
            {
                IsSuccess = true,
                Message = "Ok",
                Data = foodItems.Select(x => new FoodItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Rate = x.Rate,
                    FoodCategory = FoodCategoryEntityToVmConverter.Convertion(new FoodCategoryRepository(new RmsDbContext()).Get(x.FoodCategoryId))
                })
            };
            return Ok(response);
        }

        [Route("detail/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetList([FromUri]string id)
        {
            var result = _service.Get(id);

            if (result == null)
                return NotFound();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Food Item Detail",
                Data = new FoodItemViewModel
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    Rate = result.Rate,
                    FoodCategory = FoodCategoryEntityToVmConverter.Convertion(new FoodCategoryService(new FoodCategoryRepository(new RmsDbContext())).Get(result.FoodCategoryId))
                }
            });
        }

        [Route("add")]
        [HttpPost]
        public async Task<IHttpActionResult> Add([FromBody]FoodItemRequestModel foodItemRequestModel)
        {
            var result = _service.Insert(new FoodItem
            {
                Name = foodItemRequestModel.Name,
                Description = foodItemRequestModel.Description,
                Rate = foodItemRequestModel.Rate,
                FoodCategoryId = foodItemRequestModel.FoodCategoryId
            });

            if (result == null)
                return InternalServerError();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Food Item Created",
                Data = new FoodItemViewModel
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    Rate = foodItemRequestModel.Rate,
                    FoodCategory = FoodCategoryEntityToVmConverter.Convertion(new FoodCategoryService(new FoodCategoryRepository(new RmsDbContext())).Get(result.FoodCategoryId))
                }
            });
        }

        [Route("update/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> Update([FromBody]FoodItemRequestModel foodItemRequestModel, [FromUri]string id)
        {

            var result = _service.Update(new FoodItem
            {
                Id = id,
                Name = foodItemRequestModel.Name,
                Description = foodItemRequestModel.Description,
                Rate = foodItemRequestModel.Rate,
                FoodCategoryId = foodItemRequestModel.FoodCategoryId
            });

            if (result == null)
                return BadRequest();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Food Item Updated",
                Data = new FoodItemViewModel
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    Rate = result.Rate,
                    FoodCategory = FoodCategoryEntityToVmConverter.Convertion(new FoodCategoryService(new FoodCategoryRepository(new RmsDbContext())).Get(result.FoodCategoryId))
                }
            });
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete([FromUri]string id)
        {
            var result = _service.Remove(id);

            if (result == null)
                return InternalServerError();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = $"{result.Name} Deleted"
            });
        }

        [HttpGet]
        [Route("list/by/category/{id}")]
        public async Task<IHttpActionResult> GetFoodItemsByFoodCateogryId([FromUri]string id)
        {
            var foodItems = await _service.Get().Where(foodItem => foodItem.FoodCategoryId == id).ToListAsync();

            if (foodItems == null)
                return NotFound();

            var response = new ResponseModel
            {
                IsSuccess = true,
                Message = "Ok",
                Data = foodItems.Select(x => new FoodItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Rate = x.Rate,
                    FoodCategory = FoodCategoryEntityToVmConverter.Convertion(new FoodCategoryRepository(new RmsDbContext()).Get(x.FoodCategoryId))
                })
            };
            return Ok(response);
        }
    }
}
