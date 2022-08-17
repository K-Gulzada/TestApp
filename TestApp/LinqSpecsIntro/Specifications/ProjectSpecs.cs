using LinqSpecs;
using TestApp.LinqSpecsIntro.Models;

namespace TestApp.LinqSpecsIntro.Specifications
{
    public static class ProjectSpecs
    {
        private const int ActiveProjectMinimumStatus = 30;
        private const int ActiveProjectMaximumStatus = 40;
        public static Specification<Project> EqualAfeNumber(string number)
           => new AdHocSpecification<Project>(c => c.Number == number);

        public static Specification<Project> EqualId(int id)
           => new AdHocSpecification<Project>(c => c.Id == id);

        public static Specification<Project> DescriptionContains(string projectDescription)
            => new AdHocSpecification<Project>(c => c.Description.Contains(projectDescription));

        public static Specification<Project> ActiveProjects()
           => new AdHocSpecification<Project>(c => c.Status >= ActiveProjectMinimumStatus && c.Status <= ActiveProjectMaximumStatus);
    }
}
