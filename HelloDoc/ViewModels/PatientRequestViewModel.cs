﻿namespace HelloDoc.ViewModels
{
    public class PatientRequestViewModel
    {
        public string Symptomps  { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Street { get; set; }  
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public string Room { get; set; }
        public int Requesttypeid { get; set; } = 1;
        public short Status { get; set; } = 1;
    }
}
