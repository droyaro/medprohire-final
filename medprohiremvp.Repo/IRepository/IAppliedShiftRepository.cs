using medprohiremvp.DATA.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace medprohiremvp.Repo.IRepository
{
  public  interface IAppliedShiftRepository:IDisposable
    {
        bool InviteApplicantbyClient(ApplicantAppliedShifts appliedShift);
        List<ApplicantAppliedShiftsDays> GetAppliedShiftDays(int AppliedShift_ID);
    }
}
