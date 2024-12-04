Service: DH_Helpdesk_Schedule
Task/Work mode: 11 - ReminderNotifier
Description: Sends MailTemplate “Reminder (Initiator)” to the Initiator based on configuration for substatus.

If already using the servcie: Please replace the DH_Helpdesk_Schedule.exe file which is part of this package. 
If new service: Please read Installation Guide

Below we have proviced an example of a bat file and it's parameters:
DH_Helpdesk_Schedule.exe 11, Data Source=<server>; Initial Catalog=<database>; User Id=<id>; Password=<password>;Network Library=dbmssocn
