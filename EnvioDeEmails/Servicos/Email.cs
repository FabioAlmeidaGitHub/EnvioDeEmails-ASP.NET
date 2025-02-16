﻿using Microsoft.Extensions.Options;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EnvioDeEmails.Servicos
{
    public class Email : IEmail
    {
        private ConfiguracoesEmail _configuracoesEmail;

        public Email(IOptions<ConfiguracoesEmail> ConfiguracoesEmail)
        {
            _configuracoesEmail = ConfiguracoesEmail.Value;
        }
        public async Task EnviarEmail(string email, string assunto, string mensagem)
        {
            string destinatario = string.IsNullOrEmpty(email) ? _configuracoesEmail.Destinatario : email;

            MailMessage mail = new MailMessage
            {
                From = new MailAddress(_configuracoesEmail.Email, "NOME_EMAIL")
            };

            mail.To.Add(new MailAddress(destinatario));
            mail.Subject = assunto;
            mail.Body = mensagem;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            using (SmtpClient smtpClient = new SmtpClient(_configuracoesEmail.Endereco, _configuracoesEmail.Porta))
            {
                smtpClient.Credentials = new NetworkCredential(_configuracoesEmail.Email, _configuracoesEmail.Senha);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mail);
            }
        }
    }
}
