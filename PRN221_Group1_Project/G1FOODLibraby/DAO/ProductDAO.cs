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

        public async Task<IEnumerable<ProductResponse>> GetProducts()
        {
            List<ProductResponse> productDTOs = new List<ProductResponse>();

            try
            {
                var products = _context.Products.Include(p => p.Categogy).Include(p => p.Status).ToList();

                foreach (var product in products)
                {

                    productDTOs.Add(new ProductResponse
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        SalePercent = product.SalePercent,
                        Image = product.Image,
                        Description = product.Description,
                        Category = product.Categogy.Name,
                        Status = product.Status.Name,
                        Comments = await GetCommentByProductID(product.Id),
                        CategogyId = product.Categogy.Id
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return productDTOs;
        }

        public async Task<ProductResponse> GetProduct(Guid id)
        {
            ProductResponse productDTO = null;

            try
            {
                var product = _context.Products.Include(p => p.Categogy).Include(p => p.Status).FirstOrDefault(p => p.Id == id);

                if (product == null)
                {
                    throw new Exception("Product not exist!");
                }

                productDTO = new ProductResponse
                {
                    Id = product.Id,
                    CategogyId = product.CategogyId,
                    Name = product.Name,
                    Image = product.Image,
                    Price = product.Price,
                    Description = product?.Description,
                    Category = product.Categogy.Name,
                    Status = product.Status.Name,
                    SalePercent = product.SalePercent,
                    StatusId = product.Status.Id
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return productDTO;
        }

        public async Task AddProduct(ProductRequest productDTO)
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateProduct(ProductRequest productDTO, Guid id)
        {
            if (productDTO == null)
            {
                throw new ArgumentNullException("Product can not null!");
            }

            try
            {
                var existProduct = _context.Products.FirstOrDefault(p => p.Id == id);

                if (existProduct == null)
                {
                    throw new Exception("Product not exist!");
                }

                existProduct.Name = productDTO.Name;
                existProduct.Price = productDTO.Price;
                existProduct.SalePercent = productDTO.SalePercent;
                existProduct.Description = productDTO.Description;
                existProduct.Image = productDTO.Image;
                existProduct.StatusId = productDTO.StatusId;
                existProduct.CategogyId = productDTO.CategogyId;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteProduct(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("ID can not null!");
            }

            try
            {
                var existProduct = _context.Products.Include(p => p.Categogy).Include(p => p.Status).FirstOrDefault(p => p.Id == id);

                if (existProduct == null)
                {
                    throw new Exception("Product not exist!");
                }

                existProduct.StatusId = new Guid("C9118626-2A37-4902-96F3-9B5BD8351A2D");

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<CategoryResponse>> GetProductCategories()
        {
            try
            {
                List<CategoryResponse> categoriesDTO = new List<CategoryResponse>();

                var categorys = _context.Categogies.ToList();

                foreach (var category in categorys)
                {
                    categoriesDTO.Add(new CategoryResponse
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
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<CategogyWarehouseItemResponse>> GetCategogyWarehouseItem()
        {
            try
            {
                List<CategogyWarehouseItemResponse> categoriesDTO = new List<CategogyWarehouseItemResponse>();

                var categorys = _context.CategogyWarehouseItems.ToList();

                foreach (var category in categorys)
                {
                    categoriesDTO.Add(new CategogyWarehouseItemResponse
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
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<CommentResponse>> GetCommentByProductID(Guid guid)
        {
            try
            {
                List<CommentResponse> commentDTOs = new List<CommentResponse>();

                var comments = _context.Comments.Where(c => c.Id == guid).ToList();

                foreach (var item in comments)
                {
                    commentDTOs.Add(new CommentResponse
                    {
                        Id = item.Id,
                        AccountName = await GetUsername(item.Id),
                        Content = item.Content,
                        ParentCommentId = item.Id,
                        ParentName = await GetUsername(item.ParentCommentId ?? Guid.Empty),
                    });
                }

                return commentDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<WarehouseItemResponse>> GetWarehouseItems()
        {
            List<WarehouseItemResponse> warehouseItemDTOs = new List<WarehouseItemResponse>();

            try
            {
                var warehouseItems = _context.WarehouseItems.Include(p => p.CategogyItem).Include(p => p.Unit).ToList();

                foreach (var item in warehouseItems)
                {
                    warehouseItemDTOs.Add(new WarehouseItemResponse
                    {
                        Id = item.Id,
                        Description = item.Description,
                        Name = item.Name,
                        CategoryName = item.CategogyItem.Name,
                        Unitname = item.Unit.Name
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return warehouseItemDTOs;
        }

        public async Task AddRecipe(RecipeRequest recipeDTO)
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateRecipe(RecipeRequest recipeDTO, Guid id)
        {
            if (recipeDTO == null) throw new ArgumentNullException("Recipe can not null!");

            try
            {
                var existRecipe = _context.Recipes.FirstOrDefault(x => x.Id == id);

                if (existRecipe == null)
                {
                    throw new Exception("Recipe not exist!");
                }

                existRecipe.WarehouseItemId = recipeDTO.WarehouseItemId;
                existRecipe.Quantity = recipeDTO.Quantity;
                existRecipe.ProductId = recipeDTO.ProductId;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteRecipe(Guid guid)
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<RecipeResponse>> GetRecipeByProductId(Guid guid)
        {
            if (guid == Guid.Empty) throw new ArgumentNullException("Product ID can not null!");

            try
            {
                List<RecipeResponse> recipeDTOs = new List<RecipeResponse>();

                var recipes = _context.Recipes.Include(x => x.Product).Include(x => x.WarehouseItem).ThenInclude(w => w.Unit).Where(x => x.ProductId == guid).ToList();

                foreach (var item in recipes)
                {
                    recipeDTOs.Add(new RecipeResponse
                    {
                        Id = item.Id,
                        Quantity = item.Quantity,
                        WarehouseItemName = item.WarehouseItem.Name,
                        Unit = item.WarehouseItem.Unit.Name
                    });
                }

                return recipeDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<StatusResponse>> GetProductStatus()
        {
            try
            {
                List<StatusResponse> statusResponses = new List<StatusResponse>();

                var status = _context.ProductStatuses.ToList();

                foreach (var item in status)
                {
                    statusResponses.Add(new StatusResponse
                    {
                        Id = item.Id,
                        Description = item.Description,
                        Name = item.Name
                    });
                }

                return statusResponses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
