IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [ONGs] (
    [Id] int NOT NULL IDENTITY,
    [Nome] VARCHAR(100) NOT NULL,
    [Descricao] VARCHAR(150) NOT NULL,
    [Responsavel] VARCHAR(100) NOT NULL,
    [Email] VARCHAR(256) NOT NULL,
    CONSTRAINT [PK_ONGs] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Contato] (
    [ONGId] int NOT NULL,
    [Id] int NOT NULL IDENTITY,
    [Descricao] nvarchar(max) NULL,
    [URL] nvarchar(max) NULL,
    CONSTRAINT [PK_Contato] PRIMARY KEY ([ONGId], [Id]),
    CONSTRAINT [FK_Contato_ONGs_ONGId] FOREIGN KEY ([ONGId]) REFERENCES [ONGs] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [OngEndereco] (
    [ONGId] int NOT NULL,
    [CEP] VARCHAR(8) NOT NULL,
    [Logradouro] VARCHAR(150) NOT NULL,
    [Numero] VARCHAR(20) NOT NULL,
    [Complemento] VARCHAR(150) NULL,
    [Bairro] VARCHAR(150) NOT NULL,
    [Cidade] VARCHAR(150) NOT NULL,
    [UF] VARCHAR(2) NOT NULL,
    CONSTRAINT [PK_OngEndereco] PRIMARY KEY ([ONGId]),
    CONSTRAINT [FK_OngEndereco_ONGs_ONGId] FOREIGN KEY ([ONGId]) REFERENCES [ONGs] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Telefone] (
    [ONGId] int NOT NULL,
    [Id] int NOT NULL IDENTITY,
    [DDD] int NOT NULL,
    [Numero] int NOT NULL,
    [Tipo] int NOT NULL,
    CONSTRAINT [PK_Telefone] PRIMARY KEY ([ONGId], [Id]),
    CONSTRAINT [FK_Telefone_ONGs_ONGId] FOREIGN KEY ([ONGId]) REFERENCES [ONGs] ([Id]) ON DELETE CASCADE
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230921005913_Inicial', N'7.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TABLE [Contato];
GO

DROP TABLE [Telefone];
GO

CREATE TABLE [OngContato] (
    [ONGId] int NOT NULL,
    [Id] int NOT NULL IDENTITY,
    [Descricao] VARCHAR(100) NOT NULL,
    [URL] VARCHAR(500) NOT NULL,
    CONSTRAINT [PK_OngContato] PRIMARY KEY ([ONGId], [Id]),
    CONSTRAINT [FK_OngContato_ONGs_ONGId] FOREIGN KEY ([ONGId]) REFERENCES [ONGs] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [OngTelefone] (
    [ONGId] int NOT NULL,
    [Id] int NOT NULL IDENTITY,
    [DDD] VARCHAR(2) NOT NULL,
    [Numero] VARCHAR(9) NOT NULL,
    [Tipo] VARCHAR(10) NOT NULL,
    CONSTRAINT [PK_OngTelefone] PRIMARY KEY ([ONGId], [Id]),
    CONSTRAINT [FK_OngTelefone_ONGs_ONGId] FOREIGN KEY ([ONGId]) REFERENCES [ONGs] ([Id]) ON DELETE CASCADE
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230921232337_ConfigTabelasRelacionaisONG', N'7.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Eventos] (
    [Id] int NOT NULL IDENTITY,
    [Nome] VARCHAR(100) NOT NULL,
    [Descricao] VARCHAR(150) NOT NULL,
    [Data] DATETIME2 NOT NULL,
    [ONGId] int NULL,
    CONSTRAINT [PK_Eventos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Eventos_ONGs_ONGId] FOREIGN KEY ([ONGId]) REFERENCES [ONGs] ([Id])
);
GO

CREATE TABLE [EventoEndereco] (
    [EventoId] int NOT NULL,
    [CEP] VARCHAR(8) NOT NULL,
    [Logradouro] VARCHAR(150) NOT NULL,
    [Numero] VARCHAR(20) NOT NULL,
    [Complemento] VARCHAR(150) NULL,
    [Bairro] VARCHAR(150) NOT NULL,
    [Cidade] VARCHAR(150) NOT NULL,
    [UF] VARCHAR(2) NOT NULL,
    CONSTRAINT [PK_EventoEndereco] PRIMARY KEY ([EventoId]),
    CONSTRAINT [FK_EventoEndereco_Eventos_EventoId] FOREIGN KEY ([EventoId]) REFERENCES [Eventos] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Eventos_ONGId] ON [Eventos] ([ONGId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230922010151_ConfigControllerONGEventos', N'7.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Eventos] DROP CONSTRAINT [FK_Eventos_ONGs_ONGId];
GO

EXEC sp_rename N'[Eventos].[ONGId]', N'OngId', N'COLUMN';
GO

EXEC sp_rename N'[Eventos].[IX_Eventos_ONGId]', N'IX_Eventos_OngId', N'INDEX';
GO

DROP INDEX [IX_Eventos_OngId] ON [Eventos];
DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Eventos]') AND [c].[name] = N'OngId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Eventos] DROP CONSTRAINT [' + @var0 + '];');
UPDATE [Eventos] SET [OngId] = 0 WHERE [OngId] IS NULL;
ALTER TABLE [Eventos] ALTER COLUMN [OngId] int NOT NULL;
ALTER TABLE [Eventos] ADD DEFAULT 0 FOR [OngId];
CREATE INDEX [IX_Eventos_OngId] ON [Eventos] ([OngId]);
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[EventoEndereco]') AND [c].[name] = N'Numero');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [EventoEndereco] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [EventoEndereco] ALTER COLUMN [Numero] VARCHAR(20) NULL;
GO

ALTER TABLE [Eventos] ADD CONSTRAINT [FK_Eventos_ONGs_OngId] FOREIGN KEY ([OngId]) REFERENCES [ONGs] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230923004040_AlteraRequerimentoNumeroEnderecoEvento', N'7.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Usuarios] (
    [Id] int NOT NULL IDENTITY,
    [Nome] VARCHAR(150) NOT NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [EventoUsuario] (
    [EventosSeguidosId] int NOT NULL,
    [UsuarioId] int NOT NULL,
    CONSTRAINT [PK_EventoUsuario] PRIMARY KEY ([EventosSeguidosId], [UsuarioId]),
    CONSTRAINT [FK_EventoUsuario_Eventos_EventosSeguidosId] FOREIGN KEY ([EventosSeguidosId]) REFERENCES [Eventos] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_EventoUsuario_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [ONGUsuario] (
    [ONGsSeguidasId] int NOT NULL,
    [UsuarioId] int NOT NULL,
    CONSTRAINT [PK_ONGUsuario] PRIMARY KEY ([ONGsSeguidasId], [UsuarioId]),
    CONSTRAINT [FK_ONGUsuario_ONGs_ONGsSeguidasId] FOREIGN KEY ([ONGsSeguidasId]) REFERENCES [ONGs] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ONGUsuario_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UsuarioEndereco] (
    [UsuarioId] int NOT NULL,
    [CEP] VARCHAR(8) NOT NULL,
    [Logradouro] VARCHAR(150) NOT NULL,
    [Numero] VARCHAR(20) NOT NULL,
    [Complemento] VARCHAR(150) NULL,
    [Bairro] VARCHAR(150) NOT NULL,
    [Cidade] VARCHAR(150) NOT NULL,
    [UF] VARCHAR(2) NOT NULL,
    CONSTRAINT [PK_UsuarioEndereco] PRIMARY KEY ([UsuarioId]),
    CONSTRAINT [FK_UsuarioEndereco_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UsuarioTelefone] (
    [UsuarioId] int NOT NULL,
    [DDD] VARCHAR(2) NOT NULL,
    [Numero] VARCHAR(9) NOT NULL,
    [Tipo] VARCHAR(10) NOT NULL,
    CONSTRAINT [PK_UsuarioTelefone] PRIMARY KEY ([UsuarioId]),
    CONSTRAINT [FK_UsuarioTelefone_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_EventoUsuario_UsuarioId] ON [EventoUsuario] ([UsuarioId]);
GO

CREATE INDEX [IX_ONGUsuario_UsuarioId] ON [ONGUsuario] ([UsuarioId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230926012155_ConfigControllerUsuarios', N'7.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Aplicacoes] (
    [Id] int NOT NULL IDENTITY,
    [Usuario] VARCHAR(100) NOT NULL,
    [Senha] VARCHAR(100) NOT NULL,
    [NomeAplicacao] VARCHAR(100) NOT NULL,
    CONSTRAINT [PK_Aplicacoes] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231006002503_AutenticacaoToken', N'7.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UsuarioEndereco]') AND [c].[name] = N'Numero');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [UsuarioEndereco] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [UsuarioEndereco] ALTER COLUMN [Numero] VARCHAR(20) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231011005109_EnderecoNumNullable-Usuario', N'7.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Usuarios] ADD [TelegramId] VARCHAR(16) NULL;
GO

ALTER TABLE [UsuarioEndereco] ADD [Latitude] decimal(18,2) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [UsuarioEndereco] ADD [Longitude] decimal(18,2) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [OngEndereco] ADD [Latitude] DECIMAL(9,7) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [OngEndereco] ADD [Longitude] DECIMAL(9,7) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [EventoEndereco] ADD [Latitude] decimal(18,2) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [EventoEndereco] ADD [Longitude] decimal(18,2) NOT NULL DEFAULT 0.0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231020191656_AddTelegramId_EndLagtLgt', N'7.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UsuarioEndereco]') AND [c].[name] = N'Latitude');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [UsuarioEndereco] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [UsuarioEndereco] DROP COLUMN [Latitude];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UsuarioEndereco]') AND [c].[name] = N'Longitude');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [UsuarioEndereco] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [UsuarioEndereco] DROP COLUMN [Longitude];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OngEndereco]') AND [c].[name] = N'Latitude');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [OngEndereco] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [OngEndereco] DROP COLUMN [Latitude];
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OngEndereco]') AND [c].[name] = N'Longitude');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [OngEndereco] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [OngEndereco] DROP COLUMN [Longitude];
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[EventoEndereco]') AND [c].[name] = N'Latitude');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [EventoEndereco] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [EventoEndereco] DROP COLUMN [Latitude];
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[EventoEndereco]') AND [c].[name] = N'Longitude');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [EventoEndereco] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [EventoEndereco] DROP COLUMN [Longitude];
GO

CREATE TABLE [OngGeoLocalizacao] (
    [ONGId] int NOT NULL,
    [Latitude] DECIMAL(9,7) NOT NULL,
    [Longitude] DECIMAL(9,7) NOT NULL,
    CONSTRAINT [PK_OngGeoLocalizacao] PRIMARY KEY ([ONGId]),
    CONSTRAINT [FK_OngGeoLocalizacao_ONGs_ONGId] FOREIGN KEY ([ONGId]) REFERENCES [ONGs] ([Id]) ON DELETE CASCADE
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231020195134_AddOngGeoLocalizacao', N'7.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UsuarioEndereco]') AND [c].[name] = N'UF');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [UsuarioEndereco] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [UsuarioEndereco] ALTER COLUMN [UF] VARCHAR(2) NULL;
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UsuarioEndereco]') AND [c].[name] = N'Logradouro');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [UsuarioEndereco] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [UsuarioEndereco] ALTER COLUMN [Logradouro] VARCHAR(150) NULL;
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UsuarioEndereco]') AND [c].[name] = N'Cidade');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [UsuarioEndereco] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [UsuarioEndereco] ALTER COLUMN [Cidade] VARCHAR(150) NULL;
GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UsuarioEndereco]') AND [c].[name] = N'CEP');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [UsuarioEndereco] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [UsuarioEndereco] ALTER COLUMN [CEP] VARCHAR(8) NULL;
GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UsuarioEndereco]') AND [c].[name] = N'Bairro');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [UsuarioEndereco] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [UsuarioEndereco] ALTER COLUMN [Bairro] VARCHAR(150) NULL;
GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OngGeoLocalizacao]') AND [c].[name] = N'Longitude');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [OngGeoLocalizacao] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [OngGeoLocalizacao] ALTER COLUMN [Longitude] DECIMAL(10,8) NOT NULL;
GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OngGeoLocalizacao]') AND [c].[name] = N'Latitude');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [OngGeoLocalizacao] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [OngGeoLocalizacao] ALTER COLUMN [Latitude] DECIMAL(10,8) NOT NULL;
GO

CREATE TABLE [EventoGeoLocalizacao] (
    [EventoId] int NOT NULL,
    [Latitude] DECIMAL(10,8) NOT NULL,
    [Longitude] DECIMAL(10,8) NOT NULL,
    CONSTRAINT [PK_EventoGeoLocalizacao] PRIMARY KEY ([EventoId]),
    CONSTRAINT [FK_EventoGeoLocalizacao_Eventos_EventoId] FOREIGN KEY ([EventoId]) REFERENCES [Eventos] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UsuarioGeoLocalizacao] (
    [UsuarioId] int NOT NULL,
    [Latitude] DECIMAL(10,8) NOT NULL,
    [Longitude] DECIMAL(10,8) NOT NULL,
    CONSTRAINT [PK_UsuarioGeoLocalizacao] PRIMARY KEY ([UsuarioId]),
    CONSTRAINT [FK_UsuarioGeoLocalizacao_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE CASCADE
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231021233921_AddGeoLocalizacaoEventoUsuario', N'7.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UsuarioTelefone]') AND [c].[name] = N'Numero');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [UsuarioTelefone] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [UsuarioTelefone] ALTER COLUMN [Numero] VARCHAR(9) NULL;
GO

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UsuarioTelefone]') AND [c].[name] = N'DDD');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [UsuarioTelefone] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [UsuarioTelefone] ALTER COLUMN [DDD] VARCHAR(2) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231025003755_TelefoneNullable-Usuario', N'7.0.11');
GO

COMMIT;
GO

