using DDDPlayground.Domain.Base;

namespace DDDPlayground.Domain.Entities
{
    public class User : Entity
    {
        public string? Name { get; set; }
        public Int32 Age { get; set; }
        public DateTime BirthOfDate { get; set; }
    }
}
