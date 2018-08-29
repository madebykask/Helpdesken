--update DB from 5.3.38 to 5.3.39 version

-- New column Workinggroup_Id in tblCaseType
ALTER TABLE tblCaseType
    ADD WorkingGroup_Id INTEGER,
    FOREIGN KEY(WorkingGroup_Id) REFERENCES tblWorkingGroup(Id)

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.39'
--ROLLBACK --TMP

  

