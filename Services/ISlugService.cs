namespace BlogProjectMVC.Services
{
    public interface ISlugService
    {
        string RemoveAccents(string tittle);
        string GenerateSlug(string tittle);
        bool IsUniqueSlug(string slug);
    }

}
