USE DotNetCourseDatabase
GO

CREATE TABLE TutorialAppSchema.Posts (
    PostId INT IDENTITY(1, 1),
    UserId INT,
    PostTitle NVARCHAR(255),
    PostContent VARCHAR(MAX),
    PostCreated DATETIME,
    PostUpdated DATETIME
)

CREATE CLUSTERED INDEX cix_Posts_UserId_PostId ON TutorialAppSchema.Posts(UserId, PostId)

SELECT [PostId],
[UserId],
[PostTitle],
[PostContent],
[PostCreated],
[PostUpdated] FROM TutorialAppSchema.Posts

INSERT INTO TutorialAppSchema.Posts(
    [PostId],
    [UserId],
    [PostTitle],
    [PostContent],
    [PostCreated],
    [PostUpdated]) VALUES ()

DELETE FROM TutorialAppSchema.Posts
    WHERE PostId = 3
    AND UserId = 1005

SELECT * FROM TutorialAppSchema.Posts
    WHERE PostTitle LIKE '%search%'
     OR PostContent LIKE '%search%'