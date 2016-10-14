using System.ComponentModel.DataAnnotations;

namespace RMS.BusinessModel.Entities
{
    public class Entity
    {
        [Key]
        public string Id { get; set; }
    }
}