using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medprohiremvp.Models.ClinicalInstitution
{
    public class DashBoardViewModelold
    {
        public int ShiftCount { get; set; }
        public int AllShiftCount { get; set; }
        public int OnGoingShiftCount { get; set; }
        public int AllOnGoingShiftCount { get; set; }
        public int Contractors { get; set; }
        public int AllContractors { get; set; }
        public int Locations { get; set; }
        public int AllLocations { get; set; }
       public List<DashboardtableViewModel> ContractorsTable { get; set; }
    }
}
