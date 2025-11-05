using System.ComponentModel.DataAnnotations;

namespace Therapy_Service.Therapy.Host.Models.Base
{
    public abstract class BaseModel
    {
        [Key]
        public int ID { get; set; }
    }
}
