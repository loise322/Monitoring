﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monitoring.Models
{
    public class VocOfVariable
    {
        public int Id { get; set; }
        //Идентификатор
        public string Name { get; set; }
        public bool isBoolean { get; set; }
        public int warningThreshold { get; set; }
        public int alarmThreshold { get; set; }
        public int Priority { get; set; }
    }
}
