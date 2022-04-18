using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEMPETestWork.Models
{
    public class ExampleClass
    {
        // This attributes allows your HTML Content to be sent up
        [AllowHtml]
        [UIHint("tinymce_full_compressed")]
        public string HtmlContent { get; set; }

        public ExampleClass()
        {

        }
    }
}