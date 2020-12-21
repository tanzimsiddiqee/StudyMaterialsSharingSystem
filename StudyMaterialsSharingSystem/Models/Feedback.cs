using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string User { get; set; }
        public string Comment { get; set; }
        [DisplayFormat(DataFormatString = "{0:F}")]
        public DateTime dateTime { get; set; }
        public bool Read { get; set; }
    }
}