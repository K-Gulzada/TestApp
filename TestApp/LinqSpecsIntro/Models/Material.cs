namespace TestApp.LinqSpecsIntro.Models
{
    public class Material
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int? ProjectId { get; set; }


        #region Navigation Properties

        public Project? Project { get; set; }

        public ICollection<MaterialPart> MaterialParts { get; set; }

        #endregion
    }
}
