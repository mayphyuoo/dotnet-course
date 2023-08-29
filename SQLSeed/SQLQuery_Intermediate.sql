USE DotNetCourseDatabase
GO

-- TRUNCATE TABLE TutorialAppSchema.Computer

SELECT [Users].[UserId],
[Users].[FirstName] + ' ' + [Users].[LastName] AS FullName,
[Users].[Email],
[Users].[Gender],
[Users].[Active],
[UserJobInfo].[JobTitle],
[UserJobInfo].[Department] FROM TutorialAppSchema.Users As Users
    -- INNER JOIN
    JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
        ON Users.UserId = UserJobInfo.UserId
WHERE Users.Active = 1
ORDER BY UserId DESC


SELECT [Users].[UserId],
[Users].[FirstName] + ' ' + [Users].[LastName] AS FullName,
[UserJobInfo].[JobTitle],
[UserJobInfo].[Department],
[UserSalary].[Salary],
[Users].[Email],
[Users].[Gender],
[Users].[Active] FROM TutorialAppSchema.Users As Users
    JOIN TutorialAppSchema.UserSalary AS UserSalary
        ON Users.UserId = UserSalary.UserId
    LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
        ON Users.UserId = UserJobInfo.UserId
WHERE Users.Active = 1
ORDER BY UserId DESC


SELECT [UserSalary].[UserId],
    [UserSalary].[Salary] 
FROM TutorialAppSchema.UserSalary AS UserSalary
    WHERE EXISTS (SELECT * FROM TutorialAppSchema.UserJobInfo AS UserJobInfo
        WHERE UserJobInfo.UserId = UserSalary.UserId)
        AND UserId <> 7

SELECT [UserId],
    [Salary] FROM TutorialAppSchema.UserSalary
UNION
SELECT [UserId],
    [Salary] FROM TutorialAppSchema.UserSalary


CREATE CLUSTERED INDEX cix_UserSalary_UserId ON TutorialAppSchema.UserSalary(UserId)

CREATE NONCLUSTERED INDEX ix_UserSalary_JobTitle ON TutorialAppSchema.UserJobInfo(JobTitle) INCLUDE (Department)

-- also includes UserId because it is clustered Index
CREATE NONCLUSTERED INDEX ix_Users_JobTitle ON TutorialAppSchema.Users(Active) 
    INCLUDE ([Email], [FirstName], [LastName])
        WHERE Active = 1


SELECT ISNULL([UserJobInfo].[Department], 'No Department Listed') AS Department,
    SUM([UserSalary].[Salary]) AS Salary,
    MIN([UserSalary].[Salary]) AS MinSalary,
    MAX([UserSalary].[Salary]) AS MaxSalary,
    AVG([UserSalary].[Salary]) AS AvgSalary,
    COUNT(*) AS PeopleInDepartment,
    STRING_AGG(Users.UserId, ', ') AS UserIds
    FROM TutorialAppSchema.Users As Users
        JOIN TutorialAppSchema.UserSalary AS UserSalary
            ON Users.UserId = UserSalary.UserId
        LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
            ON Users.UserId = UserJobInfo.UserId
    WHERE Users.Active = 1
    GROUP BY [UserJobInfo].[Department]
    ORDER BY ISNULL([UserJobInfo].[Department], 'No Department Listed') DESC


SELECT [Users].[UserId],
[Users].[FirstName] + ' ' + [Users].[LastName] AS FullName,
[UserJobInfo].[JobTitle],
[UserJobInfo].[Department],
DepartmentAverage.AvgSalary,
[UserSalary].[Salary],
[Users].[Email],
[Users].[Gender],
[Users].[Active] FROM TutorialAppSchema.Users As Users
    JOIN TutorialAppSchema.UserSalary AS UserSalary
        ON Users.UserId = UserSalary.UserId
    LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
        ON Users.UserId = UserJobInfo.UserId
    -- OUTER APPLY ( -- Similar to LEFT JOIN
    CROSS APPLY ( -- Similar to JOIN
        SELECT ISNULL([UserJobInfo2].[Department], 'No Department Listed') AS Department,
        AVG([UsersSalary2].[Salary]) AS AvgSalary
        FROM TutorialAppSchema.UserSalary As UsersSalary2
            LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo2
                ON UsersSalary2.UserId = UserJobInfo2.UserId
        WHERE [UserJobInfo2].[Department] = [UserJobInfo].[Department]
        GROUP BY [UserJobInfo2].[Department]
    ) AS DepartmentAverage
WHERE Users.Active = 1
ORDER BY UserId DESC


SELECT GETDATE()

SELECT DATEADD(YEAR, -5, GETDATE())

SELECT DATEDIFF(MINUTE,DATEADD(YEAR, -5, GETDATE()), GETDATE())

ALTER TABLE TutorialAppSchema.UserSalary ADD AvgSalary DECIMAL(18,4)

SELECT * FROM TutorialAppSchema.UserSalary

UPDATE UserSalary
    SET UserSalary.AvgSalary = DepartmentAverage.AvgSalary
 FROM TutorialAppSchema.UserSalary As UserSalary
 LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
        ON UserSalary.UserId = UserJobInfo.UserId
    CROSS APPLY ( -- Similar to JOIN
        SELECT ISNULL([UserJobInfo2].[Department], 'No Department Listed') AS Department,
        AVG([UsersSalary2].[Salary]) AS AvgSalary
        FROM TutorialAppSchema.UserSalary As UsersSalary2
            LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo2
                ON UsersSalary2.UserId = UserJobInfo2.UserId
        WHERE ISNULL([UserJobInfo2].[Department], 'No Department Listed') = ISNULL([UserJobInfo].[Department], 'No Department Listed')
        GROUP BY [UserJobInfo2].[Department]
    ) AS DepartmentAverage


SELECT [Users].[UserId],
[Users].[FirstName] + ' ' + [Users].[LastName] AS FullName,
[UserJobInfo].[JobTitle],
[UserJobInfo].[Department],
UserSalary.AvgSalary,
[UserSalary].[Salary],
[Users].[Email],
[Users].[Gender],
[Users].[Active] FROM TutorialAppSchema.Users As Users
    JOIN TutorialAppSchema.UserSalary AS UserSalary
        ON Users.UserId = UserSalary.UserId
    LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
        ON Users.UserId = UserJobInfo.UserId
WHERE Users.Active = 1
ORDER BY UserId DESC
