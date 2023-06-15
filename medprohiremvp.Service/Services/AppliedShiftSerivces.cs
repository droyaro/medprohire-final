using medprohiremvp.DATA.Entity;
using medprohiremvp.Repo.IRepository;
using medprohiremvp.Service.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace medprohiremvp.Service.Services
{
   public class AppliedShiftSerivces: IAppliedShiftServices
    {
        private readonly IAppliedShiftRepository _appliedShiftRepository;
      

        public AppliedShiftSerivces(IAppliedShiftRepository appliedShiftRepository)
        {
            _appliedShiftRepository = appliedShiftRepository;
        }
        public List<ApplicantAppliedShiftsDays> GetAppliedShiftDays(int AppliedShift_ID)
        {
            return _appliedShiftRepository.GetAppliedShiftDays(AppliedShift_ID);
        }
        public void Dispose()
        {
            _appliedShiftRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
