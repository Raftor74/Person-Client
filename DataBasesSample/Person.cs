﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBasesSample
{
    enum Sex {Man,Woman,Undefined};

    abstract class Person
    {
        public string name { get; set; }
        public int age { get; set; }
        public Sex sex { get; set; }
    }
}
