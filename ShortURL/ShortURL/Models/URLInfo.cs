using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShortURL.Models
{
    public class URLInfo
    {
        public long ID { get; set; }
        [Required]
        public string LongURL { get; set; }
        public string ShortURLCode { get; set; }
        public string ShortURL { get; set; }
        public int ClicksCount { get; set; }
    }
}