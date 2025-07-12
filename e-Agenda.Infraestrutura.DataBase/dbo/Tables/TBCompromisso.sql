CREATE TABLE [dbo].[TBCompromisso] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Assunto]     NVARCHAR (100)   NOT NULL,
    [Data]        DATETIME2 (7)    NOT NULL,
    [HoraInicio]  BIGINT           NOT NULL,
    [HoraTermino] BIGINT           NOT NULL,
    [Tipo]        INT              NOT NULL,
    [Local]       NVARCHAR (100)   NULL,
    [Link]        NVARCHAR (200)   NULL,
    [Contato_Id]  UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TBContato] FOREIGN KEY ([Contato_Id]) REFERENCES [dbo].[TBContato] ([Id])
);

