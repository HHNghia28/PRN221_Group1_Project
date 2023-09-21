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
        public Task<IEnumerable<ProductDTO>> GetProducts();
        public Task<ProductDTO> GetProduct(Guid id);
        public Task<ProductDTO> AddProduct(ProductDTO productDTO);
        public Task<ProductDTO> UpdateProduct(ProductDTO productDTO);
        public Task<ProductDTO> DeleteProduct(Guid id);
        public Task<IEnumerable<CategoryDTO>> GetProductCategories();
        public Task<IEnumerable<CategogyWarehouseItemDTO>> GetCategogyWarehouseItem();
        public Task<IEnumerable<CommentDTO>> GetCommentByProductID(Guid guid);
        public Task<IEnumerable<WarehouseItemDTO>> GetWarehouseItems();
        public Task<RecipeDTO> AddRecipe(RecipeDTO recipeDTO);
        public Task<RecipeDTO> UpdateRecipe(RecipeDTO recipeDTO);
        public Task<RecipeDTO> DeleteRecipe(Guid guid);
        public Task<IEnumerable<RecipeDTO>> GetRecipeByProductId(Guid guid);
    }}
