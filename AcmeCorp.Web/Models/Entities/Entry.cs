using System.ComponentModel.DataAnnotations.Schema;

namespace AcmeCorp.Web.Models.Entities
{
    public class Entry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public int CustomerId { get; set; }
        public DateTime EntryTime { get; set; }

    }
}
