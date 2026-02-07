CREATE TABLE FlowerCategory (
    FlowerId UUID NOT NULL,
    CategoryId UUID NOT NULL,
    PRIMARY KEY (FlowerId, CategoryId)
);