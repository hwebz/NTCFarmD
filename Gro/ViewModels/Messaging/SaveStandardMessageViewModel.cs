using Gro.Business;
using System.ComponentModel.DataAnnotations;

namespace Gro.ViewModels.Messaging
{
    public class SaveStandardMessageViewModel
    {
        [Required]
        public int AreaId { get; set; }

        [Required]
        public int TypeId { get; set; }

        [ValidEmails(AllowEmpty = true)]
        public string EmailReceivers { get; set; }

        [ValidPhones]
        public string SmsReceivers { get; set; }

        [Phone]
        public string SmsSender { get; set; }

        [Required]
        [MinLength(1)]
        public string HeadLine { get; set; }

        public string MailMessage { get; set; }

        [MaxLength(160)]
        public string SmsMessage { get; set; }
    }
}
