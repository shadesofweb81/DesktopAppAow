using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingERP.Infrastructure.Entities
{
    /// <summary>
    /// Represents a payment allocation from a payment transaction to an invoice/bill
    /// </summary>
    public class TransactionPayment
    {
        [Key]
        public Guid Id { get; set; }
        
        /// <summary>
        /// The payment transaction ID
        /// </summary>
        public Guid PaymentTransactionId { get; set; }
        
        [ForeignKey("PaymentTransactionId")]
        public Transaction PaymentTransaction { get; set; }
        
        /// <summary>
        /// The invoice/bill transaction being paid
        /// </summary>
        public Guid InvoiceTransactionId { get; set; }
        
        [ForeignKey("InvoiceTransactionId")]
        public Transaction InvoiceTransaction { get; set; }
        
        /// <summary>
        /// Amount of payment allocated to this invoice/bill
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        
        /// <summary>
        /// Date of payment
        /// </summary>
        public DateTime PaymentDate { get; set; }
        
        /// <summary>
        /// Notes for this payment
        /// </summary>
        public string Notes { get; set; } = string.Empty;
        
        /// <summary>
        /// When this payment was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
} 