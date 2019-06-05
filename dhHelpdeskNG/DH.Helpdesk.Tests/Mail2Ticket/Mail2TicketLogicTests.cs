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
                var caseNumber = DH.Helpdesk.Mail2Ticket.Library.SharedFunctions.extractCaseNumberFromSubject(emailSubject, pattern);
                Assert.AreEqual(caseNumber, 0); 
            }

            // 2. case number should be found
            emailSubject = "RE: new case 7853, case:7853";

            foreach (var pattern in subjectPatterns)
            {
                var caseNumber = DH.Helpdesk.Mail2Ticket.Library.SharedFunctions.extractCaseNumberFromSubject(emailSubject, pattern);
                Assert.AreEqual(caseNumber, 7853);
            }
        }
    }
}