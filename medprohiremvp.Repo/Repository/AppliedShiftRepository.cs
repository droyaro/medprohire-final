using medprohiremvp.DATA.Entity;
using medprohiremvp.Repo.Context;
using medprohiremvp.Repo.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace medprohiremvp.Repo.Repository
{
    public class AppliedShiftRepository :IAppliedShiftRepository
    {
        protected readonly MvpDBContext _dbcontext;
   
      //  protected readonly IConfiguration _configuration;
        public AppliedShiftRepository(MvpDBContext Context)
        {
            _dbcontext = Context;
           // _configuration = configuration;
        }
        public bool InviteApplicantbyClient(ApplicantAppliedShifts appliedShift)
        {
            try
            {
                _dbcontext.ApplicantAppliedShifts.Add(appliedShift);
                _dbcontext.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }
        public List<ApplicantAppliedShiftsDays> GetAppliedShiftDays(int AppliedShift_ID)
        {
            return _dbcontext.ApplicantAppliedShiftsDays.Where(x => x.AppliedShift_ID == AppliedShift_ID).ToList();
        }
        public void Dispose()
        {
            _dbcontext.Dispose();
           
            GC.SuppressFinalize(this);
        }
    }
}
