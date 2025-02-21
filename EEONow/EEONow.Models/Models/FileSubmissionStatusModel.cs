﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEONow.Models
{
    public class FileSubmissionStatusModel
    {
        public Int32 FileSubmissionStatusId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}
