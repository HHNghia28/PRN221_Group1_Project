using G1FOODLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IProductRepository
    {
        public Task<IEnumerable<ProductResponse>> GetProducts();
        public Task<ProductResponse> GetProduct(Guid id);
        public Task AddProduct(ProductRequest productDTO);
        public Task UpdateProduct(ProductRequest productDTO, Guid id);
        public Task DeleteProduct(Guid id);
        public Task<IEnumerable<CategoryResponse>> GetProductCategories();
        public Task<IEnumerable<CategogyWarehouseItemResponse>> GetCategogyWarehouseItem();
        public Task<IEnumerable<CommentResponse>> GetCommentByProductID(Guid guid);
        public Task<IEnumerable<WarehouseItemResponse>> GetWarehouseItems();
        public Task AddRecipe(RecipeRequest recipeDTO);
        public Task UpdateRecipe(RecipeRequest recipeDTO, Guid id);
        public Task DeleteRecipe(Guid guid);
        public Task<IEnumerable<RecipeResponse>> GetRecipeByProductId(Guid guid);
        public Task<IEnumerable<StatusResponse>> GetProductStatus();
    }
}
