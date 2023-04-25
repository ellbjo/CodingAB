﻿using System.ComponentModel.DataAnnotations;

namespace CodingAB.Models
{
    public class TimeOffRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeOffType Type { get; set; }
    }
}
