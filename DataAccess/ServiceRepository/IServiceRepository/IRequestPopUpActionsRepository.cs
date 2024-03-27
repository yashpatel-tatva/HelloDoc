﻿namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IRequestPopUpActionsRepository
    {
        void AssignCase(int requestId, int physicianId, int assignby, string description);
        void BlockCase(int requestId, string description);

        void CancelCase(int requestid, int casetag, string note);
        void ClearCase(int requestid);
        void CloseCase(int requestid);
    }
}
