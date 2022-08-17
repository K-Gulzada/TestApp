using TestApp.LinqSpecsIntro.Models;

namespace TestApp.LinqSpecsIntro.Repositories
{
    public interface IProjectRepository
    {
        List<Project> GetByDescription(string projectDescription);

        List<Project> GetActiveProjects();

        Project GetByNumber(string afeNumber);

        Project GetById(int id);

    }
}
