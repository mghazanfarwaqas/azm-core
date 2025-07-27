using System.Diagnostics.CodeAnalysis;

namespace Core.Data
{
    [ExcludeFromCodeCoverage]
    public abstract class EntityBase
    {
        public int CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
