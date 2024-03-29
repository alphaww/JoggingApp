﻿using JoggingApp.Core.Weather;
using System;

namespace JoggingApp.Jogs
{
    public class JogInsertRequest
    {
        public int Distance { get; set; }
        
        public RunningTimeDto Time { get; set; }

        public Coordinates Coordinates { get; set; }
    }
}
