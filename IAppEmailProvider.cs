using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace AppVisum.Sys
{
    public interface IAppEmailProvider : IAppModelProvider
    {
        IAppEmailSender GetSender(string account);
        IEnumerable<IAppEmailAccount> GetAccounts();
    }

    public interface IAppEmailAccount
    {
        string Name { get; }
        string Email { get; }
    }

    public interface IAppEmailSender
    {
        IAppEmailProvider Provider { get; }
        IAppEmailAccount Account { get; }
        void Send(MailMessage message, string senderName = null);
    }
}
