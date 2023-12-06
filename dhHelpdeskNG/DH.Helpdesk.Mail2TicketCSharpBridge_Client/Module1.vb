Imports DH.Helpdesk.VBCSharpBridge

Module Module1

    Sub Main()
        Dim caseProcessor As New DH.Helpdesk.VBCSharpBridge.CaseExposure

        Dim caseBridge As New DH.Helpdesk.VBCSharpBridge.Models.CaseBridge()
        caseBridge.Id = 123
        caseBridge.FromEmail = "user@example.com"


        ' Call the ProcessCase method
        Dim result As DH.Helpdesk.Mail2TicketCSharpBridge.Models.CaseBridge = caseProcessor.RunBusinessRules(caseBridge)

        Console.WriteLine(result)
        Console.ReadLine()
    End Sub

End Module
