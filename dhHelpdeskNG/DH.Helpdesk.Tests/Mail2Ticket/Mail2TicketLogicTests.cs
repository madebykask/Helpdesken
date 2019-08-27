using NUnit.Framework;

namespace DH.Helpdesk.Tests.Mail2Ticket
{
    [TestFixture]
    public class Mail2TicketLogicTests
    {
        [Test]
        public void ExtractCaseNumberFromSubjectTest()
        {
            var subjectPatterns = new string[]
            {
                "case:#;ärende:#",
                //"case #;ärende #" // TODO: this will fail the test. need to find out if it was implemented for a reason or its a bug!
            };

            // 1. case number should not be found
            var emailSubject = "RE: new case has been created 7853";

            foreach (var pattern in subjectPatterns)
            {
                var caseNumber =
                    DH.Helpdesk.Library.SharedFunctions.extractCaseNumberFromSubject(emailSubject, pattern);
                Assert.AreEqual(caseNumber, 0);
            }

            // 2. case number should be found
            emailSubject = "RE: new case 7853, case:7853";

            foreach (var pattern in subjectPatterns)
            {
                var caseNumber =
                    DH.Helpdesk.Library.SharedFunctions.extractCaseNumberFromSubject(emailSubject, pattern);
                Assert.AreEqual(caseNumber, 7853);
            }
        }

        [Test]
        public void ExtractEmailAnswer()
        {
            var inputs = new[]
            {
                // text - swedish
                @"Svarar från mobilen
                  Den 17 juni 2019 14:36 skrev acct.testhelpdesk@dhsolutions.se:
                  Tilldelat case på engelska",

                // text - english
                @"Svarar från mobilen
                  On Thu, Jun 27, 2019, 4:47 PM <dhhelpdeskUTV@dhsolutions.se> wrote:
                  Ärende rubrik: Performance - save and close
                  Från: Johan Weinitz
                  Some prev email text...",

                // text - german 
                /*@"Svarar från mobilen
                  <dhhelpdeskUTV@dhsolutions.se> schrieb am Do., 27. Juni 2019, 16:47:
                  Ärende rubrik: Performance - save and close
                  Från: Johan Weinitz
                  Some prev email text...",
                */
                // text - russian
                @"Svarar från mobilen
                  чт, 27 июн. 2019 г., 16:47 <dhhelpdeskUTV@dhsolutions.se>:
                  Ärende rubrik: Performance - save and close
                  Från: Johan Weinitz
                  Some prev email text..."
            };

            //regex pattern to anayze reply text:
            var emailSeparatorPattern =
                @"^.* [0-9]{1,2}:[0-9]{1,2} (\w{0,}\s{0,})?<?(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}))>?(\s{0,}\w{0,})?:\r?$";

            //run tests
            foreach (var input in inputs)
            {
                var answer =
                    DH.Helpdesk.Library.SharedFunctions
                        .extractAnswerFromBody(input, emailSeparatorPattern) ?? string.Empty;
                Assert.AreEqual(answer, "Svarar från mobilen\r\n");
            }
        }

        [Test]
        public void ExtractEmailAnswer_German()
        {
            //german pattern:
            var input =
                @"guten Morgen
                  <dhhelpdeskUTV@dhsolutions.se> schrieb am Do., 27. Juni 2019, 16:47:
                  Ärende rubrik: Performance - save and close
                  Från: Johan Weinitz
                  Some prev email text...";

            var emailSeparatorPattern =
                @"^.* <?(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}))>? (.*)\s?[0-9]{1,2}:[0-9]{1,2}:\r?$";

            var res = DH.Helpdesk.Library.SharedFunctions.extractAnswerFromBody(input, emailSeparatorPattern) ?? string.Empty;
            Assert.AreEqual(res, "guten Morgen\r\n");
        }
    }
}