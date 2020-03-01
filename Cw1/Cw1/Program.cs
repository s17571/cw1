using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cw1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args[0] == null)
            {
                throw new ArgumentNullException();
            }

            var emails = await GetEmails(args[0]);

            foreach(var a in args)
            {
                Console.WriteLine(a);
            }

            foreach (var email in emails)
            {
                Console.WriteLine(email);
            }
        }

        static async Task<IList<string>> GetEmails(string url)
        {
            var httpClient = new HttpClient();
            var listOfEmails = new List<string>();
            HttpResponseMessage response = new HttpResponseMessage();
            
            try
            {
                response = await httpClient.GetAsync(url);
            } catch (Exception e)
            {
                Console.WriteLine("Błąd w czasie pobierania strony");
            }

            httpClient.Dispose();

            Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
            
            MatchCollection emailMatches = emailRegex.Matches(response.Content.ReadAsStringAsync().Result);

            foreach (var emailMatch in emailMatches)
            {
                if (!listOfEmails.Contains(emailMatch.ToString()))
                {
                    listOfEmails.Add(emailMatch.ToString());
                }
                
            }

            if (listOfEmails.Count == 0)
            {
                Console.WriteLine("Nie znaleziono adresów email");
                return null;
            } 

            return listOfEmails;
        }
    }
}
