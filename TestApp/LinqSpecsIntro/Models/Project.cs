namespace TestApp.LinqSpecsIntro.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        #region Navigation Properties

        public ICollection<Material> Materials { get; set; }

        public ICollection<MaterialPart> MaterialParts { get; set; }

        #endregion
    }
}
