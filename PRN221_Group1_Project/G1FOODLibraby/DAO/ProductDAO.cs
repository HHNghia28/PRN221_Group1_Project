using DataAccess.Context;
using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    internal class ProductDAO
    {
        private DBContext _context;
        private static ProductDAO instance = null;
        private static readonly object instanceLock = new object();

        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }

        public ProductDAO() => _context = new DBContext();

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            List<ProductDTO> productDTOs = new List<ProductDTO>();

            try
            {
                var products = _context.Products.Include(p => p.Categogy).Include(p => p.Status).ToList();

                foreach (var product in products)
                {

                    productDTOs.Add(new ProductDTO
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        SalePercent = product.SalePercent,
                        Image = product.Image,
                        Description = product.Description,
                        Category = product.Categogy.Name,
                        Status = product.Status.Name,
                        Comments = await GetCommentByProductID(product.Id)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }

            return productDTOs;
        }

        public async Task<ProductDTO> GetProduct(Guid id)
        {
            ProductDTO productDTO = null;

            try
            {
                var product = _context.Products.Include(p => p.Categogy).Include(p => p.Status).FirstOrDefault(p => p.Id == id);

                if (product == null)
                {
                    throw new Exception("Product not exist!");
                }

                productDTO = new ProductDTO
                {
                    Id = product.Id,
                    CategogyId = product.CategogyId,
                    Name = product.Name,
                    Image = product.Image,
                    Price = product.Price,
                    Description = product?.Description,
                    Category = product.Categogy.Name,
                    Status = product.Status.Name
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }

            return productDTO;
        }

        public async Task<ProductDTO> AddProduct(ProductDTO productDTO)
        {
            if (productDTO == null)
            {
                throw new ArgumentNullException("Product can not null!");
            }

            Guid id = Guid.NewGuid();

            try
            {
                Product product = new Product
                {
                    Id = id,
                    Image = productDTO.Image,
                    Description = productDTO.Description,
                    Name = productDTO.Name,
                    Price = productDTO.Price,
                    SalePercent = productDTO.SalePercent,
                    CategogyId = productDTO.CategogyId,
                    StatusId = productDTO.StatusId
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                productDTO.Id = id;

                return productDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }
        }

        public async Task<ProductDTO> UpdateProduct(ProductDTO productDTO)
        {
            if (productDTO == null)
            {
                throw new ArgumentNullException("Product can not null!");
            }

            try
            {
                var existProduct = _context.Products.FirstOrDefault(p => p.Id == productDTO.Id);

                if (existProduct == null)
                {
                    throw new Exception("Product not exist!");
                }

                existProduct.Name = productDTO.Name;
                existProduct.Price = productDTO.Price;
                existProduct.SalePercent = productDTO.SalePercent;
                existProduct.Description = productDTO.Description;
                existProduct.Image = productDTO.Image;

                await _context.SaveChangesAsync();

                return productDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }
        }

        public async Task<ProductDTO> DeleteProduct(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("ID can not null!");
            }

            ProductDTO productDTO = null;

            try
            {
                var existProduct = _context.Products.Include(p => p.Categogy).Include(p => p.Status).FirstOrDefault(p => p.Id == id);

                if (existProduct == null)
                {
                    throw new Exception("Product not exist!");
                }

                _context.Products.Remove(existProduct);

                await _context.SaveChangesAsync();

                productDTO = new ProductDTO
                {
                    Id = existProduct.Id,
                    CategogyId = existProduct.CategogyId,
                    Name = existProduct.Name,
                    Image = existProduct.Image,
                    Price = existProduct.Price,
                    Description = existProduct?.Description,
                    Category = existProduct.Categogy.Name,
                    Status = existProduct.Status.Name
                };

                return productDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }
        }

        public async Task<IEnumerable<CategoryDTO>> GetProductCategories()
        {
            try
            {
                List<CategoryDTO> categoriesDTO = new List<CategoryDTO>();

                var categorys = _context.Categogies.ToList();

                foreach (var category in categorys)
                {
                    categoriesDTO.Add(new CategoryDTO
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Description = category.Description
                    });
                }

                return categoriesDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }
        }

        public async Task<IEnumerable<CategogyWarehouseItemDTO>> GetCategogyWarehouseItem()
        {
            try
            {
                List<CategogyWarehouseItemDTO> categoriesDTO = new List<CategogyWarehouseItemDTO>();

                var categorys = _context.CategogyWarehouseItems.ToList();

                foreach (var category in categorys)
                {
                    categoriesDTO.Add(new CategogyWarehouseItemDTO
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Description = category.Description
                    });
                }

                return categoriesDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentByProductID(Guid guid)
        {
            try
            {
                List<CommentDTO> commentDTOs = new List<CommentDTO>();

                var comments = _context.Comments.Where(c => c.Id == guid).ToList();

                foreach (var item in comments)
                {
                    commentDTOs.Add(new CommentDTO
                    {
                        Id = item.Id,
                        AccountName = await GetUsername(item.Id),
                        AccountId = item.Id,
                        Content = item.Content,
                        ParentCommentId = item.Id,
                        ProductId = item.Id,
                        ParentName = await GetUsername(item.ParentCommentId ?? Guid.Empty),
                    });
                }

                return commentDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }
        }

        public async Task<string> GetUsername(Guid accountId)
        {
            if (accountId == Guid.Empty)
            {
                throw new ArgumentNullException("ID can not null!");
            }

            string username = "Incognito";

            try
            {
                var account = _context.Accounts.Include(a => a.Users).FirstOrDefault(a => a.Id == accountId);

                foreach (var item in account.Users)
                {
                    username = item.Name;
                    if ((bool)item.DefaultUser)
                    {
                        break;
                    }
                }

                return username;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }
        }

        public async Task<IEnumerable<WarehouseItemDTO>> GetWarehouseItems()
        {
            List<WarehouseItemDTO> warehouseItemDTOs = new List<WarehouseItemDTO>();

            try
            {
                var warehouseItems = _context.WarehouseItems.Include(p => p.CategogyItem).Include(p => p.Unit).ToList();

                foreach (var item in warehouseItems)
                {
                    warehouseItemDTOs.Add(new WarehouseItemDTO
                    {
                        Id = item.Id,
                        CategogyItemId = item.Id,
                        Description = item.Description,
                        Name = item.Name,
                        CategoryName = item.CategogyItem.Name,
                        Unitname = item.Unit.Name
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }

            return warehouseItemDTOs;
        }

        public async Task<RecipeDTO> AddRecipe(RecipeDTO recipeDTO)
        {
            if (recipeDTO == null) throw new ArgumentNullException("Recipe can not null!");

            try
            {
                Guid guid = Guid.NewGuid();

                Recipe recipe = new Recipe
                {
                    Id = guid,
                    ProductId = recipeDTO.ProductId,
                    WarehouseItemId = recipeDTO.WarehouseItemId,
                    Quantity = recipeDTO.Quantity
                };

                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();

                recipeDTO.Id = guid;

                return recipeDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }
        }

        public async Task<RecipeDTO> UpdateRecipe(RecipeDTO recipeDTO)
        {
            if (recipeDTO == null) throw new ArgumentNullException("Recipe can not null!");

            try
            {
                var existRecipe = _context.Recipes.FirstOrDefault(x => x.Id ==  recipeDTO.Id);

                if (existRecipe == null)
                {
                    throw new Exception("Recipe not exist!");
                }

                existRecipe.WarehouseItemId = recipeDTO.WarehouseItemId;
                existRecipe.Quantity = recipeDTO.Quantity;
                existRecipe.ProductId = recipeDTO.ProductId;

                await _context.SaveChangesAsync();

                return recipeDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }
        }

        public async Task<RecipeDTO> DeleteRecipe(Guid guid)
        {
            if (guid == Guid.Empty) throw new ArgumentNullException("Recipe ID can not null!");

            try
            {
                var existRecipe = _context.Recipes.FirstOrDefault(x => x.Id == guid);

                if (existRecipe == null)
                {
                    throw new Exception("Recipe not exist!");
                }

                _context.Recipes.Remove(existRecipe);

                await _context.SaveChangesAsync();

                return new RecipeDTO
                {
                    Id = existRecipe.Id,
                    ProductId = existRecipe.ProductId,
                    Quantity = existRecipe.Quantity,
                    WarehouseItemId = existRecipe.WarehouseItemId
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }
        }

        public async Task<IEnumerable<RecipeDTO>> GetRecipeByProductId(Guid guid)
        {
            if (guid == Guid.Empty) throw new ArgumentNullException("Product ID can not null!");

            try
            {
                List<RecipeDTO> recipeDTOs = new List<RecipeDTO>();

                var recipes = _context.Recipes.Include(x => x.Product).Include(x => x.WarehouseItem).ThenInclude(w => w.Unit).Where(x => x.ProductId == guid).ToList();

                foreach (var item in recipes)
                {
                    recipeDTOs.Add(new RecipeDTO
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        WarehouseItemId = item.WarehouseItemId,
                        ProductName = item.Product.Name,
                        WarehouseItemName = item.WarehouseItem.Name,
                        Unit = item.WarehouseItem.Unit.Name
                    });
                }

                return recipeDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }
        }
    }
}
