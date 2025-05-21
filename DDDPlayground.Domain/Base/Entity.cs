
namespace DDDPlayground.Domain.Base
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (Entity)obj;

            if (Id == default)
                return false;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity? left, Entity? right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);

            return left.Equals(right);
        }

        public static bool operator !=(Entity? left, Entity? right)
        {
            return !(left == right);
        }
    }

    public abstract class Entity<TPrimaryKey>
    {
        public TPrimaryKey? Id { get; protected set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (Entity<TPrimaryKey>)obj;

            if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default!))
                return false;

            return EqualityComparer<TPrimaryKey>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TPrimaryKey>.Default.GetHashCode(Id);
        }

        public static bool operator ==(Entity<TPrimaryKey>? left, Entity<TPrimaryKey>? right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);

            return left.Equals(right);
        }

        public static bool operator !=(Entity<TPrimaryKey>? left, Entity<TPrimaryKey>? right)
        {
            return !(left == right);
        }
    }
}
