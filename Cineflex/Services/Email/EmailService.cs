using Cineflex.Models;
using Cineflex.Services.Email;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;

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

        public async Task SendMovieTicketEmailAsync(string email, List<TicketEmailData> tickets)
        {
            try
            {
                var emailService = new EmailService();
                const string subject = "Cineflex - Uw film tickets";

                // Bouw de HTML voor alle tickets
                var ticketsHtml = new StringBuilder();
                foreach (var ticket in tickets)
                {
                    ticketsHtml.Append($@"
                <div class='ticket'>
                    <div class='ticket-header'>
                        <span class='ticket-icon'>🎬 </span>
                        <h3>{ticket.Name}</h3>
                    </div>
                    <div class='ticket-details'>
                        <div class='detail-row'>
                            <span class='label'>📅 Datum & Tijd </span>
                            <span class='value'> {ticket.Datetime}</span>
                        </div>
                        <div class='detail-row'>
                            <span class='label'>🚪 Zaal </span>
                            <span class='value'> {ticket.Room}</span>
                        </div>
                        <div class='detail-row'>
                            <span class='label'>💺 Stoel </span>
                            <span class='value'> {ticket.Seat}</span>
                        </div>
                    </div>
                </div>");
                }

                var message = $@"
<!DOCTYPE html>
<html lang='nl'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Film Tickets</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #1a0f0f 0%, #2d1b1b 100%);
            padding: 40px 20px;
            line-height: 1.6;
        }}
        
        .container {{
            background: linear-gradient(145deg, #2d1b1b 0%, #1f1414 100%);
            padding: 0;
            border-radius: 16px;
            max-width: 600px;
            margin: auto;
            box-shadow: 0 10px 40px rgba(0,0,0,0.5), 0 0 0 1px rgba(220,20,60,0.2);
            overflow: hidden;
        }}
        
        .header {{
            background: linear-gradient(135deg, #dc143c 0%, #8b0000 100%);
            padding: 40px 30px;
            text-align: center;
            position: relative;
            overflow: hidden;
        }}
        
        .header::before {{
            content: '';
            position: absolute;
            top: -50%;
            left: -50%;
            width: 200%;
            height: 200%;
            background: repeating-linear-gradient(
                45deg,
                transparent,
                transparent 10px,
                rgba(255,255,255,0.03) 10px,
                rgba(255,255,255,0.03) 20px
            );
            animation: slide 20s linear infinite;
        }}
        
        @keyframes slide {{
            0% {{ transform: translate(0, 0); }}
            100% {{ transform: translate(50px, 50px); }}
        }}
        
        .logo {{
            font-size: 2.5em;
            font-weight: bold;
            color: #ffffff;
            text-shadow: 0 4px 8px rgba(0,0,0,0.3);
            margin-bottom: 10px;
            position: relative;
            z-index: 1;
        }}
        
        .header-subtitle {{
            color: rgba(255,255,255,0.9);
            font-size: 1.1em;
            position: relative;
            z-index: 1;
        }}
        
        .content {{
            padding: 40px 30px;
            color: #e0e0e0;
        }}
        
        .intro {{
            background: rgba(220,20,60,0.1);
            border-left: 4px solid #dc143c;
            padding: 20px;
            margin-bottom: 30px;
            border-radius: 8px;
        }}
        
        .intro p {{
            margin: 0;
            font-size: 1.05em;
        }}
        
        .ticket-count {{
            display: inline-block;
            background: #dc143c;
            color: white;
            padding: 4px 12px;
            border-radius: 20px;
            font-weight: bold;
            font-size: 0.9em;
            margin-left: 8px;
        }}
        
        .ticket {{
            background: linear-gradient(135deg, #3d2525 0%, #2d1b1b 100%);
            border-radius: 12px;
            margin-bottom: 20px;
            overflow: hidden;
            border: 1px solid rgba(220,20,60,0.2);
            transition: transform 0.2s;
        }}
        
        .ticket-header {{
            background: linear-gradient(90deg, #dc143c 0%, #b01030 100%);
            padding: 20px;
            display: flex;
            align-items: center;
            gap: 15px;
        }}
        
        .ticket-icon {{
            font-size: 2em;
            filter: drop-shadow(0 2px 4px rgba(0,0,0,0.3));
        }}
        
        .ticket-header h3 {{
            color: white;
            font-size: 1.3em;
            margin: 0;
            text-shadow: 0 2px 4px rgba(0,0,0,0.2);
        }}
        
        .ticket-details {{
            padding: 25px;
        }}
        
        .detail-row {{
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 12px 0;
            border-bottom: 1px solid rgba(220,20,60,0.1);
        }}
        
        .detail-row:last-child {{
            border-bottom: none;
        }}
        
        .label {{
            color: #b0b0b0;
            font-size: 0.95em;
        }}
        
        .value {{
            color: #ffffff;
            font-weight: 600;
            font-size: 1.05em;
        }}
        
        .cta-section {{
            background: rgba(220,20,60,0.05);
            padding: 25px;
            border-radius: 12px;
            margin: 30px 0;
            text-align: center;
        }}
        
        .cta-button {{
            display: inline-block;
            background: linear-gradient(135deg, #dc143c 0%, #b01030 100%);
            color: white;
            padding: 15px 40px;
            text-decoration: none;
            border-radius: 30px;
            font-weight: bold;
            font-size: 1.1em;
            box-shadow: 0 4px 15px rgba(220,20,60,0.4);
            transition: all 0.3s;
        }}
        
        .tips {{
            background: rgba(255,255,255,0.03);
            border-radius: 12px;
            padding: 20px;
            margin: 30px 0;
        }}
        
        .tips h4 {{
            color: #dc143c;
            margin-bottom: 15px;
            font-size: 1.1em;
        }}
        
        .tips ul {{
            list-style: none;
            padding-left: 0;
        }}
        
        .tips li {{
            padding: 8px 0;
            padding-left: 25px;
            position: relative;
            color: #c0c0c0;
        }}
        
        .tips li::before {{
            content: '✓';
            position: absolute;
            left: 0;
            color: #dc143c;
            font-weight: bold;
        }}
        
        .footer {{
            background: #1a0f0f;
            padding: 30px;
            text-align: center;
            color: #888;
            font-size: 0.9em;
        }}
        
        .footer-links {{
            margin: 15px 0;
        }}
        
        .footer-links a {{
            color: #dc143c;
            text-decoration: none;
            margin: 0 10px;
        }}
        
        .social-icons {{
            margin: 20px 0;
        }}
        
        .social-icons span {{
            margin: 0 8px;
            font-size: 1.5em;
        }}
        
        @media only screen and (max-width: 600px) {{
            .content {{
                padding: 25px 20px;
            }}
            
            .ticket-header {{
                padding: 15px;
            }}
            
            .detail-row {{
                flex-direction: column;
                align-items: flex-start;
                gap: 5px;
            }}
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div class='logo'>CINEFLEX</div>
            <div class='header-subtitle'>Your Premium Cinema Experience</div>
        </div>
        
        <div class='content'>
            <div class='intro'>
                <p> Bedankt voor uw aankoop! U heeft <span class='ticket-count'>{tickets.Count} ticket(s)</span> succesvol gereserveerd.</p>
            </div>
            
            <h2 style='color: #dc143c; margin-bottom: 25px; font-size: 1.5em;'>Uw Tickets</h2>
            
            {ticketsHtml}
            
            <div class='tips'>
                <h4> Handige Tips</h4>
                <ul>
                    <li>Kom 15 minuten voor aanvang om uw plaatsen te vinden</li>
                    <li>Toon deze e-mail bij de ingang</li>
                    <li>Vergeet uw ID-bewijs niet voor 16+ films</li>
                    <li>Snacks en drankjes zijn verkrijgbaar bij onze bar</li>
                </ul>
            </div>
            
            <div style='text-align: center; margin: 30px 0; padding: 20px; background: rgba(255,255,255,0.02); border-radius: 12px;'>
                <p style='color: #b0b0b0; margin-bottom: 10px;'>Wij wensen u alvast een geweldige filmervaring toe! 🍿</p>
                <p style='color: #e0e0e0; font-weight: 600;'>Geniet van de film!</p>
            </div>
            
            <p style='color: #a0a0a0; font-style: italic; margin-top: 25px;'>
                Met vriendelijke groet,<br>
                <strong style='color: #dc143c;'>Team Cineflex</strong>
            </p>
        </div>
        
        <div class='footer'>
            <div class='footer-links'>
            </div>
            <p style='margin-top: 15px;'>© 2025 Cineflex. Alle rechten voorbehouden.</p>
            <p style='margin-top: 10px; font-size: 0.85em; color: #666;'>
                Deze e-mail is automatisch gegenereerd, reageren is niet mogelijk.
            </p>
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
    }
}
