using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoApp.Models
{
    [Table("DimEmployee")]
    public class Employee
    {
        [Key]
        public int EmployeeKey { get; set; }
        public String? FirstName { get; set; }
        public String? LastName { get; set; }
        public Boolean NameStyle { get; set; }
        public Boolean CurrentFlag { get; set; }
        public Boolean SalesPersonFlag { get; set; }
        public String? Title { get; set; }
        public DateOnly HireDate { get; set; }
        public DateOnly StartDate { get; set; }
        public String? LoginID { get; set; }
        public String? EmailAddress { get; set; }
        public String? Phone { get; set; }
        public String? DepartmentName { get; set; }
        public String? Status { get; set; }
        public byte[]? EmployeePhoto { get; set; }

    }
}
