using System.Diagnostics.CodeAnalysis;

namespace Core.Data
{
    [ExcludeFromCodeCoverage]
    public abstract class EntityBase
    {
        public Guid CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
