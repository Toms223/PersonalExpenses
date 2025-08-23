CREATE TABLE "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(255),
    "Email" VARCHAR(255),
    "HashedPassword" VARCHAR(255),
    "Limit" INT DEFAULT 0,
);

CREATE TABLE "Categories" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "Color" VARCHAR(6) NOT NULL
);


CREATE TABLE "Expenses" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "Amount" REAL NOT NULL ,
    "Date" DATE NOT NULL ,
    "UserId" INT,
    "CategoryId" INT NULL,
    "Continuous" BOOLEAN,
    "Period" INT,
    "Fixed" BOOLEAN,
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id"),
    FOREIGN KEY ("CategoryId") REFERENCES "Categories"("Id")
);

