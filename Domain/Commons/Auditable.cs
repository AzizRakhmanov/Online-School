using System.ComponentModel.DataAnnotations;

namespace Domain.Commons
{
    public class Auditable
    {
        [Key]
        public Guid Id { get; set; }
    }
}
