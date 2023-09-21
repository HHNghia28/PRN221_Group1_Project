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
        public Task<ProductDTO> AddProduct(ProductDTO productDTO) => ProductDAO.Instance.AddProduct(productDTO);

        public Task<RecipeDTO> AddRecipe(RecipeDTO recipeDTO) => ProductDAO.Instance.AddRecipe(recipeDTO);

        public Task<ProductDTO> DeleteProduct(Guid id) => ProductDAO.Instance.DeleteProduct(id);

        public Task<RecipeDTO> DeleteRecipe(Guid guid) => ProductDAO.Instance.DeleteRecipe(guid);

        public Task<IEnumerable<CategogyWarehouseItemDTO>> GetCategogyWarehouseItem() => ProductDAO.Instance.GetCategogyWarehouseItem();

        public Task<IEnumerable<CommentDTO>> GetCommentByProductID(Guid guid) => ProductDAO.Instance.GetCommentByProductID(guid);

        public Task<ProductDTO> GetProduct(Guid id) => ProductDAO.Instance.GetProduct(id);

        public Task<IEnumerable<CategoryDTO>> GetProductCategories() => ProductDAO.Instance.GetProductCategories();

        public Task<IEnumerable<ProductDTO>> GetProducts() => ProductDAO.Instance.GetProducts();

        public Task<IEnumerable<RecipeDTO>> GetRecipeByProductId(Guid guid) => ProductDAO.Instance.GetRecipeByProductId(guid);

        public Task<IEnumerable<WarehouseItemDTO>> GetWarehouseItems() => ProductDAO.Instance.GetWarehouseItems();

        public Task<ProductDTO> UpdateProduct(ProductDTO productDTO) => ProductDAO.Instance.UpdateProduct(productDTO);

        public Task<RecipeDTO> UpdateRecipe(RecipeDTO recipeDTO) => ProductDAO.Instance.UpdateRecipe(recipeDTO);
    }
}
