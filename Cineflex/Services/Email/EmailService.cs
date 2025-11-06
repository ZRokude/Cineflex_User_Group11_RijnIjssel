using Cineflex.Services.Email;
using MailKit.Net.Smtp;
using MimeKit;

namespace Cinelexx.Services.Email
{
    public class EmailService : IEmailService
    {

        private const string From = "info@Cineflex.nl";
        private const string SmtpServer = "smtp.gmail.com";
        private const int Port = 587;
        private const string Username = "lekkerbiertje0@gmail.com";
        private const string Password = "kvnd bqoz meop ixrv";
        

        public async Task SendEmailAsync(string to, string subject, string htmlBody)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("Team Cineflex", From));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlBody };

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(SmtpServer, Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(Username, Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task SendEmailAsyncWithFirstLoginCode(string email, string code)
        {
            try
            {
                var emailService = new EmailService();
                const string subject = "Cineflex - Eerste keer inloggen"; //JON: title of the mail

                var message = $@"
            <!DOCTYPE html>
            <html lang='nl'>
            <head>
                <meta charset='UTF-8'>
                <title>Inlogcode</title>
                
        <style>
            body {{
                font-family: Arial, sans-serif;
                color: #e0e0e0;
                background-color: #1a0f0f;
                padding: 20px;
            }}
            .container {{
                background-color: #2d1b1b;
                padding: 30px;
                border-radius: 12px;
                max-width: 600px;
                margin: auto;
                box-shadow: 0 4px 15px rgba(139,0,0,0.3);
                border: 1px solid #8b0000;
            }}
            h2 {{
                color: #dc143c;
                text-shadow: 0 2px 4px rgba(220,20,60,0.3);
            }}
            .code {{
                font-size: 1.5em;
                font-weight: bold;
                background: linear-gradient(135deg, #8b0000, #a0001a);
                color: #ffffff;
                padding: 15px;
                border-radius: 8px;
                text-align: center;
                margin: 20px 0;
                letter-spacing: 2px;
                box-shadow: 0 2px 8px rgba(139,0,0,0.4);
                border: 2px solid #dc143c;
            }}
            p {{
                color: #d0d0d0;
                line-height: 1.6;
            }}
            strong {{
                color: #dc143c;
            }}
            .footer {{
                margin-top: 30px;
                font-size: 12px;
                color: #999;
                border-top: 1px solid #4d2d2d;
                padding-top: 15px;
            }}
        </style>
            </head>
            <body>
                <div class='container'>
                    <h2>Welkom bij Covadis RentACar</h2>
                    <p>Beste medewerker,</p>
                    <p>Je persoonlijke inlogcode is:</p>
                    <div class='code'>{code}</div>
                    <p>Deze code is <strong>1 uur geldig</strong>.</p>
                    <p>Gebruik deze om in te loggen bij je account.</p>
                    <p>Met vriendelijke groet,<br>Team Covadis</p>
                    <div class='footer'>
                        © 2025 Covadis RentACar. Alle rechten voorbehouden.
                    </div>
                </div>
            </body>
            </html>";

                await emailService.SendEmailAsync(email, subject, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }

        }

        public async Task SendPasswordChangedEmailAsync(string email, DateTime _timeNow)
        {
            try
            {
                var emailService = new EmailService();
                const string subject = "Cineflex - Uw wachtwoord is veranderd"; //JON: title of the mail


                var message = $@"
            <!DOCTYPE html>
            <html lang='nl'>
            <head>
                <meta charset='UTF-8'>
                <title>Wachtwoord gewijzigd</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                color: #e0e0e0;
                background-color: #1a0f0f;
                padding: 20px;
            }}
            .container {{
                background-color: #2d1b1b;
                padding: 30px;
                border-radius: 12px;
                max-width: 600px;
                margin: auto;
                box-shadow: 0 4px 15px rgba(139,0,0,0.3);
                border: 1px solid #8b0000;
            }}
            h2 {{
                color: #dc143c;
                text-shadow: 0 2px 4px rgba(220,20,60,0.3);
            }}
            .code {{
                font-size: 1.5em;
                font-weight: bold;
                background: linear-gradient(135deg, #8b0000, #a0001a);
                color: #ffffff;
                padding: 15px;
                border-radius: 8px;
                text-align: center;
                margin: 20px 0;
                letter-spacing: 2px;
                box-shadow: 0 2px 8px rgba(139,0,0,0.4);
                border: 2px solid #dc143c;
            }}
            p {{
                color: #d0d0d0;
                line-height: 1.6;
            }}
            strong {{
                color: #dc143c;
            }}
            .footer {{
                margin-top: 30px;
                font-size: 12px;
                color: #999;
                border-top: 1px solid #4d2d2d;
                padding-top: 15px;
            }}
        </style>
            </head>
            <body>
                <div class='container'>
                    <h2>Wachtwoord gewijzigd</h2>
                    <p>Beste gebruiker,</p>
                    <p>Uw wachtwoord is zojuist gewijzigd op <strong>{_timeNow}</strong>.</p>
                    <p>Was u dit niet? Neem dan <strong>direct</strong> contact op met onze klantenservice.</p>
                    <p>Met vriendelijke groet,<br>Team Cinefle</p>
                    <div class='footer'>
                        © 2025 Cinefle. Alle rechten voorbehouden.
                    </div>
                </div>
            </body>
            </html>";

                await SendEmailAsync(email, subject, message); //JON: send the mail
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }

        public async Task SendForgotPasswordEmailAsync(string email, string code)
        {
            try
            {
                var emailService = new EmailService();
                const string subject = "Cineflex - Wachtwoord vergeten"; //JON:title of the email
                                                                         //JON: the e-mail it self
                var message = $@"
    <!DOCTYPE html>
    <html lang='nl'>
    <head>
        <meta charset='UTF-8'>
        <title>Herstelcode</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                color: #e0e0e0;
                background-color: #1a0f0f;
                padding: 20px;
            }}
            .container {{
                background-color: #2d1b1b;
                padding: 30px;
                border-radius: 12px;
                max-width: 600px;
                margin: auto;
                box-shadow: 0 4px 15px rgba(139,0,0,0.3);
                border: 1px solid #8b0000;
            }}
            h2 {{
                color: #dc143c;
                text-shadow: 0 2px 4px rgba(220,20,60,0.3);
            }}
            .code {{
                font-size: 1.5em;
                font-weight: bold;
                background: linear-gradient(135deg, #8b0000, #a0001a);
                color: #ffffff;
                padding: 15px;
                border-radius: 8px;
                text-align: center;
                margin: 20px 0;
                letter-spacing: 2px;
                box-shadow: 0 2px 8px rgba(139,0,0,0.4);
                border: 2px solid #dc143c;
            }}
            p {{
                color: #d0d0d0;
                line-height: 1.6;
            }}
            strong {{
                color: #dc143c;
            }}
            .footer {{
                margin-top: 30px;
                font-size: 12px;
                color: #999;
                border-top: 1px solid #4d2d2d;
                padding-top: 15px;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <h2>Herstelcode voor je account</h2>
            <p>Beste gebruiker,</p>
            <p>Hier is je herstelcode:</p>
            <div class='code'>{code}</div>
            <p>Deze code is <strong>1 uur geldig</strong>.</p>
            <p>Gebruik deze code om je wachtwoord te herstellen.</p>
            <p>Met vriendelijke groet,<br> Team Cineflex</p>
            <div class='footer'>
                © 2025 Cineflex. Alle rechten voorbehouden.
            </div>
        </div>
    </body>
    </html>";
                await SendEmailAsync(email, subject, message); //JON: send the mail
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }

        public async Task SendWelkomEmailAsync(string email)
        {
            try
            {

                var emailService = new EmailService();
                const string subject = "Welkom bij Cinelex!"; //JON: title of the mail
                var message = @"
				<!DOCTYPE html>
				<html lang='nl'>
				<head>
				    <meta charset='UTF-8'>
				    <title>Welkom bij Cinelexx</title>
				    <style>
            body {
                font-family: Arial, sans-serif;
                color: #e0e0e0;
                background-color: #1a0f0f;
                padding: 20px;
            }
            .container {
                background-color: #2d1b1b;
                padding: 30px;
                border-radius: 12px;
                max-width: 600px;
                margin: auto;
                box-shadow: 0 4px 15px rgba(139,0,0,0.3);
                border: 1px solid #8b0000;
            }
            h2 {
                color: #dc143c;
                text-shadow: 0 2px 4px rgba(220,20,60,0.3);
            }
            .code {
                font-size: 1.5em;
                font-weight: bold;
                background: linear-gradient(135deg, #8b0000, #a0001a);
                color: #ffffff;
                padding: 15px;
                border-radius: 8px;
                text-align: center;
                margin: 20px 0;
                letter-spacing: 2px;
                box-shadow: 0 2px 8px rgba(139,0,0,0.4);
                border: 2px solid #dc143c;
            }
            p {
                color: #d0d0d0;
                line-height: 1.6;
            }
            strong {
                color: #dc143c;
            }
            .footer {
                margin-top: 30px;
                font-size: 12px;
                color: #999;
                border-top: 1px solid #4d2d2d;
                padding-top: 15px;
            }
        </style>
				</head>
				<body>
				    <div class='container'>
                        <h2>Welkom bij Cinelex!</h2>
                        <p>Beste gebruiker,</p>
                        <p>Welkom bij Cinelex! Bij ons geniet u van de nieuwste films op het grote scherm, met de beste sfeer en beleving.</p>
                        <p>Wij houden u op de hoogte van filmreleases, speciale acties en evenementen. Uw ultieme filmervaring begint hier!</p>
                        <p>Veel kijkplezier en tot snel in onze bioscoop!</p>
                        <p>Met vriendelijke groet,<br>Team Cinelex</p>
                        <div class='footer'>
                            © 2025 Cinelex. Alle rechten voorbehouden.
                        </div>
				    </div>
				</body>
				</html>";
                await SendEmailAsync(email, subject, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }

        }

        //public async Task SendMovieTicketEmailAsync(string email, Ticket ticket)
        //{
        //    try
        //    {
        //        var emailService = new EmailService();
        //        const string subject = "Cineflex - Uw film ticket"; //JON:title of the email

        //        //JON: the e-mail it self
        //        var message = $@"
        //    <!DOCTYPE html>
        //    <html lang='nl'>
        //    <head>
        //        <meta charset='UTF-8'>
        //        <title>Herstelcode</title>
        //        <style>
        //            body {{
        //                font-family: Arial, sans-serif;
        //                color: #333;
        //                background-color: #f7f7f7;
        //                padding: 20px;
        //            }}
        //            .container {{
        //                background-color: #ffffff;
        //                padding: 20px;
        //                border-radius: 8px;
        //                max-width: 600px;
        //                margin: auto;
        //                box-shadow: 0 2px 5px rgba(0,0,0,0.1);
        //            }}
        //            h2 {{
        //                color: #F5AF00;
        //            }}
        //            .code {{
        //                font-size: 1.5em;
        //                font-weight: bold;
        //                background-color: #eef;
        //                padding: 10px;
        //                border-radius: 6px;
        //                text-align: center;
        //                margin: 20px 0;
        //            }}
        //            .footer {{
        //                margin-top: 30px;
        //                font-size: 12px;
        //                color: #777;
        //            }}
        //        </style>
        //    </head>
        //    <body>
        //        <div class='container'>
        //            <img src=""https://s3.eu-central-1.amazonaws.com/covadis/media/Logos/logo_covadis_2016.png"" alt=""Cineflex Logo"" style=""max-width: 200px; margin-bottom: 20px;"">
        //            <h2>Uw film ticket</h2>
        //            <p>Bedankt voor uw aankoop! Uw ticket voor de film is succesvol gereserveerd. Hieronder vindt u de details van uw reservering:,</p>
        //            <p>informatie van uw ticket:</p>
        //                   <div class='code'>
        //                        <strong>Film:</strong> {ticket.Name}<br>
        //                        <strong>Datum & Tijd:</strong> {ticket.Datetime}<br>
        //                        <strong>Zaal:</strong> {ticket.Room}<br>
        //                        <strong>Stoel:</strong> {ticket.Seat}
        //                    </div>
        //            <p>Wij wensen u alvast een fijne filmervaring toe!.</p>
        //            <p>Met vriendelijke groet,<br> Team Cineflex</p>
        //            <div class='footer'>
        //                © 2025 Cineflex. Alle rechten voorbehouden.
        //            </div>
        //        </div>
        //    </body>
        //    </html>";


        //        await SendEmailAsync(email, subject, message); //JON: send the mail
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Failed to send email: {ex.Message}");
        //    }

        //}

    }
}
