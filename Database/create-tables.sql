IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[projects]') AND type in (N'U'))
DROP TABLE [dbo].[projects]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[users]') AND type in (N'U'))
DROP TABLE [dbo].[users]
GO

CREATE TABLE users(
    umap_id int IDENTITY(1,1) PRIMARY KEY,
    email varchar(255) NOT NULL,
    display_name varchar(255),
    jira_id varchar(255) NOT NULL,
    wrike_id varchar(255) NOT NULL,
    created_date DATETIME,
    updated_date DATETIME
);

CREATE TABLE projects(
    pmap_id int IDENTITY(1,1) PRIMARY KEY,
    project_name varchar(255) NOT NULL,
    jira_id varchar(255) NOT NULL,
    wrike_id varchar(255) NOT NULL,
    created_date DATETIME,
    updated_date DATETIME
);