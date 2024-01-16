
CREATE TABLE transactions (
  "ID" SERIAL PRIMARY KEY,
  "SellerId" INT NOT NULL,
  "BuyerId" INT NOT NULL,
  "AdId" INT NOT NULL,
  "PriceActual" INTEGER,
  "DateTime" TIMESTAMP
);

