using DataAccess.DAO;
using G1FOODLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        public Task AddProduct(ProductRequest productDTO) => ProductDAO.Instance.AddProduct(productDTO);

        public Task AddRecipe(RecipeRequest recipeDTO) => ProductDAO.Instance.AddRecipe(recipeDTO);

        public Task DeleteProduct(Guid id) => ProductDAO.Instance.DeleteProduct(id);

        public Task DeleteRecipe(Guid guid) => ProductDAO.Instance.DeleteRecipe(guid);

        public Task<IEnumerable<CategogyWarehouseItemResponse>> GetCategogyWarehouseItem() => ProductDAO.Instance.GetCategogyWarehouseItem();

        public Task<IEnumerable<CommentResponse>> GetCommentByProductID(Guid guid) => ProductDAO.Instance.GetCommentByProductID(guid);

        public Task<ProductResponse> GetProduct(Guid id) => ProductDAO.Instance.GetProduct(id);

        public Task<IEnumerable<CategoryResponse>> GetProductCategories() => ProductDAO.Instance.GetProductCategories();

        public Task<IEnumerable<ProductResponse>> GetProducts() => ProductDAO.Instance.GetProducts();

        public Task<IEnumerable<StatusResponse>> GetProductStatus() => ProductDAO.Instance.GetProductStatus();

        public Task<IEnumerable<RecipeResponse>> GetRecipeByProductId(Guid guid) => ProductDAO.Instance.GetRecipeByProductId(guid);

        public Task<IEnumerable<WarehouseItemResponse>> GetWarehouseItems() => ProductDAO.Instance.GetWarehouseItems();

        public Task UpdateProduct(ProductRequest productDTO, Guid id) => ProductDAO.Instance.UpdateProduct(productDTO, id);

        public Task UpdateRecipe(RecipeRequest recipeDTO, Guid id) => ProductDAO.Instance.UpdateRecipe(recipeDTO, id);
    }
}
