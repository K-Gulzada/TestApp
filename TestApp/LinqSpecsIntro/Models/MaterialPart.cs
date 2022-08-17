namespace TestApp.LinqSpecsIntro.Models
{
    public class MaterialPart
    {
        public int Id { get; set; }

        public float Quantity { get; set; }

        public int MaterialId { get; set; }

        public int ProjectId { get; set; }

        public int UnitOfMeasureId { get; set; }

        #region Navigation Propertoies

        public Material Material { get; set; }

        public Project Project { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        #endregion
    }
}
