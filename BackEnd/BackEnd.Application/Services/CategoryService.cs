using BackEnd.Application.Dtos.CategoriesDtos;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;

namespace BackEnd.Application.Services
{
    public class CategoryService
    {
        private readonly ICategoriesRepository _categoryRepository;

        public CategoryService(ICategoriesRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryReadDto>> ReadCategories()
        {
            var categories = await _categoryRepository.ReadCategories();
            return categories.Select(c => new CategoryReadDto
            {
                PK_CATEGORY = c.PK_CATEGORY,
                CATEGORY_NAME = c.CATEGORY_NAME,
                STATUS = c.STATUS,
                CREATION_DATE = c.CREATION_DATE.HasValue ? c.CREATION_DATE.Value.ToString("dd/MM/yyyy HH:mm") : null
            });
        }

        public async Task<IEnumerable<CategoryReadDto>> SearchCategory(string text)
        {
            var categories = await _categoryRepository.SearchCategory(text);
            return categories.Select(c => new CategoryReadDto
            {
                PK_CATEGORY = c.PK_CATEGORY,
                CATEGORY_NAME = c.CATEGORY_NAME,
                STATUS = c.STATUS,
                CREATION_DATE = c.CREATION_DATE.HasValue ? c.CREATION_DATE.Value.ToString("dd/MM/yyyy HH:mm") : null
            });
        }

        public async Task<IEnumerable<CategorySelectDto>> SelectCategories()
        {
            var categories = await _categoryRepository.ReadCategories();
            return categories.Select(c => new CategorySelectDto
            {
                PK_CATEGORY = c.PK_CATEGORY,
                CATEGORY_NAME = c.CATEGORY_NAME
            });
        }

        public async Task<string> CreateCategory(CategoryCreateDto createDto)
        {
            var category = new Categories
            {
                CATEGORY_NAME = createDto.CATEGORY_NAME,
            };
            return await _categoryRepository.CreateCategory(category);
        }

        public async Task<string> UpdateCategory(CategoryUpdateDto updateDto)
        {
            var category = new Categories
            {
                PK_CATEGORY = updateDto.PK_CATEGORY,
                CATEGORY_NAME = updateDto.CATEGORY_NAME,
            };
            return await _categoryRepository.UpdateCategory(category);
        }
        public async Task EnabledCategory(int id)
        {
            await _categoryRepository.EnabledCategory(id);
        }

        public async Task DisabledCategory(int id)
        {
            await _categoryRepository.DisabledCategory(id);
        }
    }
}
