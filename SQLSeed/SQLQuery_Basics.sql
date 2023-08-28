USE DotNetCourseDatabase
GO

CREATE TABLE TutorialAppSchema.Computer
(
    ComputerId INT Identity(1,1) PRIMARY KEY
    , Motherboard NVARCHAR(50)
    , CPUCores INT
    , HasWifi BIT
    , HasLTE BIT
    , ReleaseDate DATETIME
    , Price DECIMAL(18, 4)
    , VideoCard NVARCHAR(50)
)
GO

SELECT * FROM TutorialAppSchema.Computer

-- allows insert automatically incremented ID manually
-- SET IDENTITY_INSERT TutorialAppSchema.Computer ON

INSERT INTO TutorialAppSchema.Computer (
[Motherboard],
[CPUCores],
[HasWifi],
[HasLTE],
[ReleaseDate],
[Price],
[VideoCard]
) VALUES (
    'Sample-Motherboard',
    4,
    1,
    0,
    '2022-01-01',
    1000,
    'Sample-Videocard'
)

DELETE FROM TutorialAppSchema.Computer WHERE ComputerId = 2

UPDATE TutorialAppSchema.Computer SET CPUCores = NULL WHERE ComputerId = 1

SELECT [Motherboard],
ISNULL([CPUCores], 4) AS CPUCores,
[HasWifi],
[HasLTE],
[ReleaseDate],
[Price],
[VideoCard] FROM TutorialAppSchema.Computer
    ORDER BY ReleaseDate