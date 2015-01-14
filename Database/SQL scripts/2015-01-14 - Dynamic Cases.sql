if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id where syscolumns.name = N'Modal' and sysobjects.name = N'tblForm')
begin		
  Alter Table tblForm Add Modal bit default 0		
end

GO

update tblForm set Modal = 0
