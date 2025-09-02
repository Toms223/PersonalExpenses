CREATE TABLE "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(255),
    "Email" VARCHAR(255),
    "HashedPassword" VARCHAR(255),
    "Limit" INT DEFAULT 0
);

CREATE TABLE "Categories" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL,
    "Name" VARCHAR(255) NOT NULL,
    "Color" VARCHAR(6) NOT NULL,
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id")
);


CREATE TABLE "Expenses" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "Amount" REAL NOT NULL ,
    "Date" DATE NOT NULL ,
    "UserId" INT,
    "CategoryId" INT DEFAULT 0,
    "Continuous" BOOLEAN,
    "Period" INT,
    "Fixed" BOOLEAN,
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id"),
    FOREIGN KEY ("CategoryId") REFERENCES "Categories"("Id")
);

