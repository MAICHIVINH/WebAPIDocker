namespace Application.DTOs
{

    public class CreateBudgetDto
    {
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Period { get; set; } = "MONTHLY";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class BudgetDto : CreateBudgetDto
    {
        public int Id { get; set; }
    }


    public class UpdateBudgetDto
    {
        public decimal Amount { get; set; }
        public string Period { get; set; } = "MONTHLY";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int CategoryId { get; set; }
    }

    public class BudgetUsageDto
    {
        public int BudgetId { get; set; }
        public int CategoryId { get; set; }
        public decimal LimitAmount { get; set; }
        public decimal SpentAmount { get; set; }
        public decimal Remaining { get; set; }
        public double Percent { get; set; }
    }
}

