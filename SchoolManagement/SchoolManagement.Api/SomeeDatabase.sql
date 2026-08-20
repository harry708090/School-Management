-- IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
-- BEGIN
--     CREATE TABLE [__EFMigrationsHistory] (
--         [MigrationId] nvarchar(150) NOT NULL,
--         [ProductVersion] nvarchar(32) NOT NULL,
--         CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
--     );
-- END;
-- GO

-- BEGIN TRANSACTION;
-- CREATE TABLE [Classes] (
--     [Id] int NOT NULL IDENTITY,
--     [Name] nvarchar(max) NOT NULL,
--     CONSTRAINT [PK_Classes] PRIMARY KEY ([Id])
-- );

-- CREATE TABLE [Students] (
--     [Id] int NOT NULL IDENTITY,
--     [FirstName] nvarchar(max) NOT NULL,
--     [LastName] nvarchar(max) NOT NULL,
--     [StudentNumber] nvarchar(max) NOT NULL,
--     [SchoolClassId] int NOT NULL,
--     CONSTRAINT [PK_Students] PRIMARY KEY ([Id]),
--     CONSTRAINT [FK_Students_Classes_SchoolClassId] FOREIGN KEY ([SchoolClassId]) REFERENCES [Classes] ([Id]) ON DELETE NO ACTION
-- );

-- CREATE TABLE [Subjects] (
--     [Id] int NOT NULL IDENTITY,
--     [Name] nvarchar(max) NOT NULL,
--     [SchoolClassId] int NOT NULL,
--     CONSTRAINT [PK_Subjects] PRIMARY KEY ([Id]),
--     CONSTRAINT [FK_Subjects_Classes_SchoolClassId] FOREIGN KEY ([SchoolClassId]) REFERENCES [Classes] ([Id]) ON DELETE NO ACTION
-- );

-- CREATE TABLE [StudentSubject] (
--     [StudentsId] int NOT NULL,
--     [SubjectsId] int NOT NULL,
--     CONSTRAINT [PK_StudentSubject] PRIMARY KEY ([StudentsId], [SubjectsId]),
--     CONSTRAINT [FK_StudentSubject_Students_StudentsId] FOREIGN KEY ([StudentsId]) REFERENCES [Students] ([Id]) ON DELETE CASCADE,
--     CONSTRAINT [FK_StudentSubject_Subjects_SubjectsId] FOREIGN KEY ([SubjectsId]) REFERENCES [Subjects] ([Id]) ON DELETE CASCADE
-- );

-- CREATE INDEX [IX_Students_SchoolClassId] ON [Students] ([SchoolClassId]);

-- CREATE INDEX [IX_StudentSubject_SubjectsId] ON [StudentSubject] ([SubjectsId]);

-- CREATE INDEX [IX_Subjects_SchoolClassId] ON [Subjects] ([SchoolClassId]);

-- INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
-- VALUES (N'20260811161449_InitialCreate', N'10.0.10');

-- COMMIT;
-- GO

