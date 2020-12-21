using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Models
{
    public class Request
    {
        [Key]
        public string ID { get; set; }
        public string MaterialID { get; set; }
        public string Material { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public bool Read { get; set; }
        public DateTime dateTime { get; set; }
        public ICollection<Reply> Replies { get; set; }
    }

    public class Reply
    {
        [Key]
        public string ID { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
        public DateTime dateTime { get; set; }
        public string RequestID { get; set; }
        public Request Request { get; set; }
    }
}
