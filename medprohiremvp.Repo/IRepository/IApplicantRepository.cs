using System;
using System.Collections.Generic;
using System.Text;
using medprohiremvp.DATA.Entity;

namespace medprohiremvp.Repo.IRepository
{
    public interface IApplicantRepository:IBaseRepository<Applicants>
    {
        bool SaveApplicant(Applicants applicant, List<ApplicantCertificates> certificates, List<ApplicantSpecialities> specialities, List<ApplicantWorkHistories> workHistory);
        bool EditApplocant(Applicants applicant, List<ApplicantCertificates> certificates, List<ApplicantSpecialities> specialities, List<ApplicantWorkHistories> workHistory);
        
    }
}
