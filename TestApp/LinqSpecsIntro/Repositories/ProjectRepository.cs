using TestApp.LinqSpecsIntro.Models;
using TestApp.LinqSpecsIntro.Specifications;

namespace TestApp.LinqSpecsIntro.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        public List<Project> GetActiveProjects()
        {
            var allProjects = CreateDefaultData();
            var specification = ProjectSpecs.ActiveProjects();
            var activeProjects = allProjects.AsQueryable().Where(specification.ToExpression());

            foreach (var project in activeProjects)
            {
                Console.WriteLine(project.Status);
            }

            return activeProjects.ToList();
        }

        public List<Project> GetByDescription(string projectDescription)
        {
            var allProjects = CreateDefaultData();
            var specification = ProjectSpecs.DescriptionContains(projectDescription);
            var projects = allProjects.AsQueryable().Where(specification.ToExpression());

            foreach (var project in projects)
            {
                Console.WriteLine(project.Description);
            }

            return projects.ToList();
        }

        public Project GetByNumber(string number)
        {
            var allProjects = CreateDefaultData();
            var specification = ProjectSpecs.EqualAfeNumber(number);
            var project = allProjects.AsQueryable().SingleOrDefault(specification.ToExpression());

            Console.WriteLine(project.Number + " " + project.Description);

            return project;
        }

        public Project GetById(int id)
        {
            var allProjects = CreateDefaultData();
            var specification = ProjectSpecs.EqualId(id);
            var project = allProjects.AsQueryable().SingleOrDefault(specification.ToExpression());

            if (project == null)
            {
                throw new NullReferenceException();
            }

            foreach (var material in project.Materials)
            {
                Console.WriteLine(material.Id + " " + material.Description);
            }

            return project;
        }

        public void Test()
        {
            var allProjects = CreateDefaultData();

            var materialParts = GetDefaultMaterialParts();
            var unitOfMeasures = GetDefaultUnitOfMeasures();

            var projects = (from p in allProjects
                           join m in materialParts on p.Id equals m.ProjectId
                           join u in unitOfMeasures on m.UnitOfMeasureId equals u.Id
                           select new { Number = p.Number, Description = p.Description, materialPartQuantity = m.Quantity, UoM = u.Name }).Distinct();


            foreach (var project in projects)
            {
                Console.WriteLine(project.Number + " " + project.Description + " " + project.materialPartQuantity + " " + project.UoM);
            }
        }

        public List<Project> CreateDefaultData()
        {

            var materialsForProject_1 = new List<Material> {    new Material { Id = 101, Description = "Material Description_1" },
                                                                new Material { Id = 102, Description = "Material Description_2" }};

            var materialsForProject_2 = new List<Material> { new Material { Id = 103, Description = "Material Description_3" },
                                                             new Material { Id = 104, Description = "Material Description_4" } };

            var materialsForProject_3 = new List<Material> { new Material { Id = 105, Description = "Material Description_5" } };

            var materialsForProject_4 = new List<Material> { new Material { Id = 106, Description = "Material Description_6" } };

            var materialsForProject_5 = new List<Material> { new Material { Id = 107, Description = "Material Description_7" } };


            var project_1 = new Project { Id = 1, Number = "1111", Description = "Description_1", Status = 30, Materials = materialsForProject_1 };
            var project_2 = new Project { Id = 2, Number = "2222", Description = "Description_2", Status = 40, Materials = materialsForProject_2 };
            var project_3 = new Project { Id = 3, Number = "3333", Description = "Description_1", Status = 50, Materials = materialsForProject_3 };
            var project_4 = new Project { Id = 4, Number = "4444", Description = "Description_4", Status = 20, Materials = materialsForProject_4 };
            var project_5 = new Project { Id = 5, Number = "5555", Description = "Description_5", Status = 80, Materials = materialsForProject_5 };

            List<Project> projects = new List<Project>();
            projects.Add(project_1);
            projects.Add(project_2);
            projects.Add(project_3);
            projects.Add(project_4);
            projects.Add(project_5);

            return projects;
        }

        public List<MaterialPart> GetDefaultMaterialParts()
        {
            var materialPart_1 = new MaterialPart { Id = 301, Quantity = 15, MaterialId = 101, ProjectId = 1, UnitOfMeasureId = 201 };
            var materialPart_2 = new MaterialPart { Id = 302, Quantity = 20, MaterialId = 102, ProjectId = 1, UnitOfMeasureId = 201 };
            var materialPart_3 = new MaterialPart { Id = 303, Quantity = 25, MaterialId = 103, ProjectId = 2, UnitOfMeasureId = 202 };

            var materiialParts = new List<MaterialPart>();
            materiialParts.Add(materialPart_1);
            materiialParts.Add(materialPart_2);
            materiialParts.Add(materialPart_3);

            return materiialParts;
        }

        public List<UnitOfMeasure> GetDefaultUnitOfMeasures()
        {
            var unitOfMeasure_1 = new UnitOfMeasure { Id = 201, Code = "kg", Name = "kilogram" };
            var unitOfMeasure_2 = new UnitOfMeasure { Id = 201, Code = "g", Name = "gram" };
            var unitOfMeasure_3 = new UnitOfMeasure { Id = 201, Code = "t", Name = "tonne" };

            var unitOfMeasures = new List<UnitOfMeasure>();
            unitOfMeasures.Add(unitOfMeasure_1);
            unitOfMeasures.Add(unitOfMeasure_2);
            unitOfMeasures.Add(unitOfMeasure_3);

            return unitOfMeasures;
        }

    }
}
