using Azure.Core;
using DAL.DTO.Request;
using DAL.DTO.Response;
using DAL.Models;
using DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<CategoryResponce> CreatCategoryAsync(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
           var result=await _categoryRepository.CreateAsync(category);
            var result2 = result.Adapt<CategoryResponce>();
            return result2;
        }

        public async Task<List<CategoryResponce>> GetAllCategoriesForAdmin()
        {
            var category = await _categoryRepository.GetAllAsync();

            var responce = category.Adapt<List<CategoryResponce>>();
            return responce;


        }

        public async Task<List<CategoryUserResponce>> GetAllCategoriesForUser(string lang = "en")
        {
            var category = await _categoryRepository.GetAllAsync();

            var responce = category.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<CategoryUserResponce>>();

             
            return responce;


        }


        public async Task<BaseResponce> DeleteCategoryAsync(int id)
        {
            try
            {
                var user = await _categoryRepository.FindById(id);
                if (user is null)
                {
                    return new BaseResponce
                    {
                        Success = "flase",
                        Message = "no user with this id"
                    };
                }
                await _categoryRepository.DeleteCategoryAsync(user);
                return new BaseResponce
                {
                    Success = "true",
                    Message = "user deleted succeflluy"
                };


            }
            catch (Exception ex)
            {
                return new BaseResponce
                {
                    Success = "flase",
                    Message = "delete not done",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public async Task<BaseResponce> ToggleStatus(int id)
        {
            try
            {
                var category = await _categoryRepository.FindById(id);
                if (category is null)
                {
                    return new BaseResponce
                    {
                        Success = "false",
                        Message = "Category Not Found"
                    };
                }

                category.Status = category.Status == Status.Active ? Status.InActive : Status.Active;
                await _categoryRepository.UpdateAsync(category);
                return new BaseResponce
                {
                    Success = "true",
                    Message = "Category Updated Successfully"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponce
                {
                    Success = "false",
                    Message = "Cant delete category",
                    Errors = new List<string> { ex.Message }
                };

            }
        }
        public async Task<BaseResponce> UpdateCategoryAsync(int id, CategoryRequest request)
        {
            try
            {
                var category = await _categoryRepository.FindById(id);
                if (category is null)
                {
                    return new BaseResponce
                    {
                        Success = "false",
                        Message = "Category Not Found"
                    };
                }
                if (request.Translations != null)
                {
                    foreach (var tmslation in request.Translations)
                    {
                        var existing = category.Translations.FirstOrDefault(t => t.Language == tmslation.Language);

                        if (existing is not null)
                        {
                            existing.Name = tmslation.Name;
                        }
                        else
                        {
                            return new BaseResponce
                            {
                                Success = "true",
                                Message = $" Language {tmslation.Language} not supported"
                            };
                        }
                    }
                }

                await _categoryRepository.UpdateAsync(category);
                return new BaseResponce
                {
                    Success = "true",
                    Message = "Category Updated Successfully"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponce
                {
                    Success = "false",
                    Message = "CCan not update category ",
                    Errors = new List<string> { ex.Message }
                };
            }
        }


    }
}
