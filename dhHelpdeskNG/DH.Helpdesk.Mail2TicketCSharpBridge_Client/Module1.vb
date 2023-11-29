Imports DH.Helpdesk.Mail2TicketCSharpBridge

Module Module1

    Sub Main()
        Dim caseProcessor As New DH.Helpdesk.Mail2TicketCSharpBridge.CaseExposure

        Dim caseBridge As New DH.Helpdesk.Mail2TicketCSharpBridge.Models.CaseBridge()
        caseBridge.Id = 123
        caseBridge.FromEmail = "user@example.com"


        ' Call the ProcessCase method
        Dim result As String = caseProcessor.RunBusinessRules(caseBridge)

        Console.WriteLine(result)
        Console.ReadLine()
    End Sub

End Module
