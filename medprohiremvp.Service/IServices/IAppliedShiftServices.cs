using medprohiremvp.DATA.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace medprohiremvp.Service.IServices
{
  public  interface IAppliedShiftServices:IDisposable
    {
        List<ApplicantAppliedShiftsDays> GetAppliedShiftDays(int AppliedShift_ID);
    }
}
