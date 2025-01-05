using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knot_Simulator
{
    // NAME     : Configuration
    // PURPOSE  : This Configuration class is a model for representing the columns of the Configuration table in the database.
    public class Configuration
    {
        public string ConfigurationId { get; set; }
        public int SimulationTime { get; set; }
        public string StationName { get; set; }
        public string OrderNumber { get; set; }
        public int OrderQuantity { get; set; }
        public decimal OrderAmount { get; set; }
        public string ProductName { get; set; }
        public string BatchNumber { get; set; }
        public int FinishedGoodsTrayMaxQty { get; set; }
        public string PartName1 { get; set; }
        public string PartName2 { get; set; }
        public string PartName3 { get; set; }
        public string PartName4 { get; set; }
        public string PartName5 { get; set; }
        public string PartName6 { get; set; }
        public int ReplQtyPart1 { get; set; }
        public int ReplQtyPart2 { get; set; }
        public int ReplQtyPart3 { get; set; }
        public int ReplQtyPart4 { get; set; }
        public int ReplQtyPart5 { get; set; }
        public int ReplQtyPart6 { get; set; }
        public int PartThresholdQty { get; set; }
        public string EmployeeName { get; set; }
        public double EmployeeSkillLevel { get; set; }
    }
}
