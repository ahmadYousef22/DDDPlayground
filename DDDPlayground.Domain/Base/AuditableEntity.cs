
namespace DDDPlayground.Domain.Base
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedDate { get; protected set; }
        public string? CreatedBy { get; protected set; }

        public DateTime? ModifiedDate { get; protected set; }
        public string? ModifiedBy { get; protected set; }

        protected AuditableEntity()
        {
            CreatedDate = DateTime.UtcNow;
        }

        public void SetCreated(string user)
        {
            CreatedBy = user;
            CreatedDate = DateTime.UtcNow;
        }

        public void SetModified(string user)
        {
            ModifiedBy = user;
            ModifiedDate = DateTime.UtcNow;
        }
    }
}
