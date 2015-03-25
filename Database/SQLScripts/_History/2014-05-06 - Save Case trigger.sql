

IF EXISTS (SELECT name FROM sysobjects
      WHERE name = 'TR_CaseSave' AND type = 'TR')
   DROP TRIGGER TR_CaseSave

