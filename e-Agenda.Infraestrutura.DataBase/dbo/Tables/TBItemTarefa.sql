CREATE TABLE [dbo].[TBItemTarefa] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [Titulo]    NVARCHAR (100)   NOT NULL,
    [Concluido] BIT              NOT NULL,
    [Tarefa_Id] UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TBItemTarefa_TBTarefa] FOREIGN KEY ([Tarefa_Id]) REFERENCES [dbo].[TBTarefa] ([Id])
);

