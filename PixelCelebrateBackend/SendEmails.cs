//No namespace:

using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Pkcs;
using PixelCelebrateBackend.Database;
using PixelCelebrateBackend.MailTrapConfig;
using PixelCelebrateBackend.Models;
using PixelCelebrateBackend.Services;

public class SendEmails : BackgroundService
{
    //Logger:
    private ILogger<SendEmails> _logger;

    //BD:
    PixelCelebrateContext pixelCelebrateContext = new PixelCelebrateContext();

    //Pentru mail:
    private readonly IMailService _mailService;
    //Mail data private:
    //private MailData mailData;
    public MailData mailData;


    //Controller:
    public SendEmails(ILogger<SendEmails> logger, IMailService _MailService)
    {
        _logger = logger;

        //Pentru schimbare mail data:
        _mailService = _MailService;

        //Creare email:
        //mailData = new MailData();
    }


    //Pentru async:
    public async Task<bool> SendMailAsync(MailData mailData)
    {
        return await _mailService.SendMailAsync(mailData);
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //Console.WriteLine("Time now: " + DateTime.Now);

            var userData = pixelCelebrateContext.Users;

            //All Users that need to receive email:

            //Get number of days:
            string dataFromFile = "";            
            int dataFromFileNumber = 0;
            StreamReader reader = null;

            try
            {
                reader = new StreamReader("../NumberOfDays.txt");

                //Console.WriteLine("Data from file is: ");
                dataFromFile = reader.ReadLine();
                //Console.WriteLine(dataFromFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();

                //Extract number:
                try
                {
                    dataFromFileNumber = Convert.ToInt32(dataFromFile);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            //Data cu number of days inainte:

            //Data de acum:
            //var dateNow = DateTime.Now; //Pentru nimeni in general;

            //Data pentru testare diverse: In anul 2023, not now:
            var dateNow = new DateTime(2023, 1, 9, 0, 0, 0);  //Pentru: Admin12, Angajat56; Cu 1 zi   inainte;
            //var dateNow = new DateTime(2023, 1, 8, 0, 0, 0);  //Pentru: Admin12, Angajat56; Cu 2 zile inainte;
            //var dateNow = new DateTime(2023, 2, 9, 0, 0, 0);  //Pentru: Angajat12         ; Cu 1 zi   inainte;
            //var dateNow = new DateTime(2023, 6, 20, 0, 0, 0); //Pentru: Angajat78         ; Cu 1 zi   inainte;

            //Cand a fost pornita aplicatia:
            Console.WriteLine("The application is going on: " + dateNow + ".");

            dateNow = dateNow.AddDays(dataFromFileNumber);
            var monthWithDelay = dateNow.Month;
            var dayWithDelay = dateNow.Day;

            Console.WriteLine("Now with delay: " + monthWithDelay + ", " + dayWithDelay + ".");

            //Verify if there is a birthday in n days:
            //Console.WriteLine("All users every x seconds:");
            //String[] userWithBirthdays = null;
            List<string> userWithoutBirthdays = new List<string>();
            List<string> userWithoutBirthdaysEmails = new List<string>();
            List<User> userWithoutBirthdaysBoth = new List<User>();
            List<string> userWithBirthdays = new List<string>();

            //Verific cine are si cine nu are:
            foreach (var allUserData in userData)
            {
                //Console.WriteLine(allUserData.UserName);
                //Daca nu este una din month sau day, nu are ziua de nastere acum:
                if(allUserData.Birthday.Month == monthWithDelay 
                   && allUserData.Birthday.Day == dayWithDelay)
                {
                    userWithBirthdays.Add(allUserData.UserName);
                }
                else
                {
                    userWithoutBirthdays.Add(allUserData.UserName);
                    userWithoutBirthdaysEmails.Add(allUserData.Email);
                    userWithoutBirthdaysBoth.Add(allUserData);
                }
            }

            //Cui sa trimita:
            string messageForUsers = "";
            foreach (var allUsersWithoutBirthdays in userWithBirthdays)
            {
                messageForUsers = messageForUsers + allUsersWithoutBirthdays.ToString() + ", ";
            }

            //Atunci se trimit mailuri la ceilalti care nu au zile de nastere acum: Despre cei care au:
            Console.WriteLine("Sending emails about " + messageForUsers + ":");
            foreach (var allUsersWithoutBirthdays in userWithoutBirthdaysBoth) //userWithoutBirthdays)
            {
                //Nu sunt emailuri de trimis asa:
                if(messageForUsers == "")
                {
                    Console.WriteLine("There are no birthdays today, so no emails were sent.");
                    break;
                }

                //Se trimite mail la angajati si administratori:
                Console.WriteLine("Sending email to " + allUsersWithoutBirthdays.UserName.ToString() + " .");

                //SEND EMAIL:

                //Creare email: 1 pentru fiecare:
                mailData = new MailData();

                //Email:
                mailData.EmailToId = allUsersWithoutBirthdays.Email;
                //Username:
                mailData.EmailToName = allUsersWithoutBirthdays.UserName;
                //Subiect:
                mailData.EmailSubject = "From Pixel Celebrate!";
                //Continut:
                mailData.EmailBody = "     This is a generated message. In " + dataFromFileNumber + 
                                     " days from now, the birthday/s of " + messageForUsers + "is happening. Maybe prepare" +
                                     " a present for them to show you apreciation! Good luck until then!";



                //1) De n ori: SINCRON:
                //bool emailSentSync = _mailService.SendMail(mailData);


                //2) De n ori: ASINCRON:
                //Merge maxim 5 trimise; Pe acest port (2525)
                Task<bool> emailSentAsync = SendMailAsync(mailData);
            }



            //1 second = 1000;
            //Every 10 seconds:

            //In seconds:

            //2 minute:
            var howMuchDelay = 120;

            //1 day:
            //var howMuchDelay = 86400;

            await Task.Delay(howMuchDelay * 1000, stoppingToken);
        }
    }
}
