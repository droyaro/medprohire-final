using System;
using System.Collections.Generic;
using System.Text;

namespace medprohiremvp.DATA.Entity
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class EncryptedAttribute : Attribute
    { }

}
