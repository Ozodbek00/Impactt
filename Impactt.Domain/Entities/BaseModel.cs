namespace Impactt.Domain.Entities
{
    public abstract class BaseModel
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
