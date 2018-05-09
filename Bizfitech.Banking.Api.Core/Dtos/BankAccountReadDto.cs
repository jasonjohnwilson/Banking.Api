using System;

namespace Bizfitech.Banking.Api.Core.Dtos
{
    public class BankAccountReadDto
    {
        public string Bank { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string SortCode { get; set; }
        public double Balance { get; set; }
        public double Overdraft { get; set; }
        public double AvailableBalance { get; set; }
    }
}
