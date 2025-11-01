CREATE TYPE UserRole AS Enum ('User', 'Admin');

CREATE TYPE LanguageCode AS ENUM ('UA', 'RO');

CREATE TABLE Flowers (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    Slug VARCHAR(255) UNIQUE NOT NULL,
    IsDeleted BOOLEAN DEFAULT FALSE,
    DeletedAtUtc TIMESTAMP,
    Price DECIMAL CHECK ( Price > 0 ) NOT NULL,
    Description VARCHAR(500) NOT NULL
);

CREATE TABLE FlowerName (
    FlowerId UUID NOT NULL,
    LanguageCode LanguageCode NOT NULL,
    Name VARCHAR(255) NOT NULL,
    PRIMARY KEY (FlowerId, LanguageCode),
    CONSTRAINT fk_flower FOREIGN KEY (FlowerId) REFERENCES Flowers(Id) ON DELETE CASCADE
);

CREATE TABLE Categories (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    Name VARCHAR(255) NOT NULL
);

CREATE TABLE CategoryName (
    CategoryId UUID NOT NULL,
    LanguageCode LanguageCode NOT NULL,
    Name VARCHAR(255) NOT NULL,
    PRIMARY KEY (CategoryId, LanguageCode),
    CONSTRAINT fk_category FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE CASCADE
);

CREATE TABLE Users (
    Id UUID PRIMARY KEY,
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(255),
    PasswordSalt TEXT NOT NULL,
    PasswordHash TEXT NOT NULL,
    Role UserRole NOT NULL DEFAULT 'User',
    IsEmailVerified BOOLEAN DEFAULT FALSE,
    IsDeleted BOOLEAN DEFAULT FALSE,
    DeletedAtUtc TIMESTAMP
);

CREATE TABLE UserFlower (
    FlowerId UUID NOT NULL,
    UserId UUID NOT NULL,
    PRIMARY KEY(FlowerId, UserId),
    CONSTRAINT fk_flower FOREIGN KEY (FlowerId) REFERENCES Flowers(Id) ON DELETE CASCADE,
    CONSTRAINT fk_user FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

CREATE TABLE Orders (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    TotalPrice DECIMAL NOT NULL,
    UserId UUID NOT NULL,
    CONSTRAINT fk_user FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

CREATE TABLE FlowerOrder (
    FlowerId UUID NOT NULL,
    OrderId UUID NOT NULL,
    Quantity SMALLINT NOT NULL CHECK (Quantity > 0),
    PRIMARY KEY(FlowerId, OrderId),
    CONSTRAINT fk_flower FOREIGN KEY (FlowerId) REFERENCES Flowers(Id) ON DELETE CASCADE,
    CONSTRAINT fk_order FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE
);

CREATE INDEX flowername_on_flowerid_fkey ON FlowerName(FlowerId);

CREATE INDEX categoryname_on_categoryid_fkey ON CategoryName(CategoryId);

CREATE INDEX userflower_on_userid_fkey ON UserFlower(UserId);
CREATE INDEX userflower_on_flowerid_fkey ON UserFlower(FlowerId);

CREATE INDEX orders_on_userid_fkey ON Orders(UserId);

CREATE INDEX flowerorder_on_flowerid_fkey ON FlowerOrder(FlowerId);
CREATE INDEX flowerorder_on_orderid_fkey ON FlowerOrder(OrderId);