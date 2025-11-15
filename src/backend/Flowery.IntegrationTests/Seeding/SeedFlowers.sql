INSERT INTO flowers(id, slug, isdeleted, deletedatutc, price, description)
VALUES ('7c77c1b8-0889-4685-9d84-6eb4500f8926', 'rose', false, null, 10.0, 'A white rose'),
       ('8bb48737-eb02-403e-8503-d8405d3bbeb4', 'daisy', false, null, 5.0, 'Beautiful daisy!'),
       ('21ce1f3a-5313-4fd5-a487-9df482601dc4', 'chrysanthemum', false, null, 5.0, 'Oh, this Chrysantemum!');

INSERT INTO flowername(flowerid, languagecode, name)
VALUES ('7c77c1b8-0889-4685-9d84-6eb4500f8926', 'UA', 'Роза'),
       ('8bb48737-eb02-403e-8503-d8405d3bbeb4', 'UA', 'Ромашка'),
       ('21ce1f3a-5313-4fd5-a487-9df482601dc4', 'UA', 'Хризантема');
