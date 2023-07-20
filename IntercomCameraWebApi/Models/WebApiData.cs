using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntercomCameraWebApi.Models
{
    public class WebApiData
    {

        public WebApiData()
        {
            CreatedAt =  DateTime.UtcNow;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Block { get; set; }
        public string IP { get; set; }
        public string IntercomIP { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string URL { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
    }

}
