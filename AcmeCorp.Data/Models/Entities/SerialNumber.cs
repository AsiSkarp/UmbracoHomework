﻿using System.ComponentModel.DataAnnotations.Schema;

namespace AcmeCorp.Data.Models.Entities
{
    public class SerialNumber
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Serial { get; set; }
    }
}
