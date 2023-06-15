using System;
using System.Collections.Generic;
using System.Text;

namespace medprohiremvp.Service.SignSend
{
    public interface ISignature
    {
        string Geturlsignature(string filename, string filepath, string email, string name, Guid userId, string returnurl, int xposition, int yposition, int pagenamber, int? emp_xposition, int? emp_yposition, int? emp_pagenamber);
        string Downloadsignfile(string Envelope_id,string user_id , string filename);
    }
}
