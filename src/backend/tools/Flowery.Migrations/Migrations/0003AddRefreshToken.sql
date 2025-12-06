CREATE TABLE refreshtoken(
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    Token varchar(88) UNIQUE NOT NULL,
    CreatedAt TIMESTAMP NOT NULL,
    ExpiresAt TIMESTAMP NOT NULL,
    IsRevoked BOOLEAN NOT NULL DEFAULT false,
    UserId UUID NOT NULL
);