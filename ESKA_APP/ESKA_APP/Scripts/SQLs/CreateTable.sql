DROP TABLE "Tm_Process";
CREATE COLUMN TABLE "Tm_Process" (
    "ProcessName" NVARCHAR(150),
    "Description" NVARCHAR(250),
    "IsActive" CHAR(1),

    "CreatedDate" TIMESTAMP,
    "CreatedUser" INTEGER,
    "ModifiedDate" TIMESTAMP,
    "ModifiedUser" INTEGER
);
