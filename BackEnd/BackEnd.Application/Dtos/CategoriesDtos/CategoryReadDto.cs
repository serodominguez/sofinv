namespace BackEnd.Application.Dtos.CategoriesDtos
{
    public class CategoryReadDto
    {
        public int PK_CATEGORY { get; set; }
        public string? CATEGORY_NAME { get; set; }
        public string? STATUS { get; set; }
        public string? CREATION_DATE { get; set; }
    }
}
