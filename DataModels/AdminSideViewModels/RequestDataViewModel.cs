namespace DataModels.AdminSideViewModels
{
    public class RequestDataViewModel
    {
        public int RequestId { get; set; }
        public enum Requestby
        {
            first,
            Patient,
            FriendorFamily,
            Concierge,
            BusinessPartner,
            VIP
        }

        public enum Statuses
        {
            first,
            New,            //New           Unassigned
            Pending,        //Pending       Accepted
            Active,         //Active        MDEnRoute
            Conclude,       //Active        MDOnSite
            To_Close,       //To-close      Cancelled
            Unpaid          //Conclude      Conclude
                            //To-close      CancelledByPatient
                            //To-close      Closed
                            //Unpaid        Unpaid
                            //Clear
        }
        public string Statusname(int i)
        {
            int index = 1;
            if (i == 1) index = 1;
            else if (i == 2) index = 2;
            else if (i == 3 || i == 7 || i == 8) index = 5;
            else if (i == 4 || i == 5) index = 3;
            else if (i == 6) index = 4;
            else if (i == 9) index = 6;
            string status = ((Statuses)index).ToString();
            return status;
        }

        public string RequestTypeName(int by)
        {
            string By = ((Requestby)by).ToString();
            return By;
        }
        public int RequesttypeID { get; set; }
        public string Notes { get; set; }
        public string ConfirmationNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime PatientDOB { get; set; }
        public string PatientMobile { get; set; }
        public string PatientEmail { get; set; }
        public string Region { get; set; }
        public string BusinessName { get; set; }
        public string Room { get; set; }
        public short Status { get; set; }
        public string pageredirectto { get; set; }
    }
}
