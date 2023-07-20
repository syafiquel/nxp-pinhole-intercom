using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntercomCameraWebApi.Models
{
    public class Intercom
    {

        public Intercom()
        {
            CreatedAt =  DateTime.UtcNow;
        }

        public int Id { get; set; }
        public string URL { get; set; }
        public string Status { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
    }

}
