using DataAccess.Repository;
using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProductRepository _productRepository;

        public ProductController() => _productRepository = new ProductRepository();

        [HttpGet("getProducts")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetProducts();
                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get all products successful!",
                    Data = products
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getProduct")]
        public async Task<IActionResult> GetProduct(string id)
        {
            try
            {
                var products = await _productRepository.GetProduct(new Guid(id));
                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get product successful!",
                    Data = products
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO productDTO)
        {
            try
            {
                var products = await _productRepository.AddProduct(productDTO);

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Add product successful!",
                    Data = products
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDTO productDTO)
        {
            try
            {
                var products = await _productRepository.UpdateProduct(productDTO);

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Update product successful!",
                    Data = products
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("deleteProduct")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                var products = await _productRepository.DeleteProduct(new Guid(id));

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Delete product successful!",
                    Data = products
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getRecipes")]
        public async Task<IActionResult> GetRecipes(string id)
        {
            try
            {
                var recipes = await _productRepository.GetRecipeByProductId(new Guid(id));
                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get recipes successful!",
                    Data = recipes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("addRecipe")]
        public async Task<IActionResult> AddRecipe([FromBody] RecipeDTO recipeDTO)
        {
            try
            {
                var recipes = await _productRepository.AddRecipe(recipeDTO);

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Add recipe successful!",
                    Data = recipes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("updateRecipe")]
        public async Task<IActionResult> UpdateRecipe([FromBody] RecipeDTO recipeDTO)
        {
            try
            {
                var recipes = await _productRepository.UpdateRecipe(recipeDTO);

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Update recipe successful!",
                    Data = recipes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("deleteRecipe")]
        public async Task<IActionResult> DeleteRecipe(string id)
        {
            try
            {
                var recipes = await _productRepository.DeleteRecipe(new Guid(id));

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Delete recipe successful!",
                    Data = recipes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getProductCategories")]
        public async Task<IActionResult> GetProductCategogies()
        {
            try
            {
                var categories = await _productRepository.GetProductCategories();

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get category of product successful!",
                    Data = categories
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getCategogyWarehouseItem")]
        public async Task<IActionResult> GetCategogyWarehouseItem()
        {
            try
            {
                var categories = await _productRepository.GetCategogyWarehouseItem();

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get category of warehouse item successful!",
                    Data = categories
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getWarehouseItems")]
        public async Task<IActionResult> GetWarehouseItems()
        {
            try
            {
                var categories = await _productRepository.GetWarehouseItems();

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get warehouse item successful!",
                    Data = categories
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getComment")]
        public async Task<IActionResult> GetComment(string id)
        {
            try
            {
                var categories = await _productRepository.GetCommentByProductID(new Guid(id));

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get comment successful!",
                    Data = categories
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
