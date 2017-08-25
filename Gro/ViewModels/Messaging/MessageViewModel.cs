using System;

namespace Gro.ViewModels.Messaging
{
    public class MessageViewModel
    {
        public string CategoryDescription { get; set; }

        public string HeadLine { get; set; }

        public string MailMessage { get; set; }

        public int MessageId { get; set; }

        public string MsgText { get; set; }

        public string TypeDescription { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
