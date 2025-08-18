CREATE TABLE "Users" (
     "Id" SERIAL PRIMARY KEY,
     "Name" VARCHAR(255),
     "Email" VARCHAR(255),
     "HashedPassword" VARCHAR(255)
);

CREATE TABLE "Expenses" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(255),
    "Amount" REAL NOT NULL ,
    "Date" DATE NOT NULL ,
    "UserId" INT,
    "Continuous" BOOLEAN,
    "Period" INT,
    "Fixed" BOOLEAN,
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id")
);

