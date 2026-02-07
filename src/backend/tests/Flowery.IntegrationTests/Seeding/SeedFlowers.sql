INSERT INTO flowers(id, slug, isdeleted, deletedatutc, price, description)
VALUES ('7c77c1b8-0889-4685-9d84-6eb4500f8926', 'rose', false, null, 10.0, 'A white rose'),
       ('8bb48737-eb02-403e-8503-d8405d3bbeb4', 'daisy', false, null, 5.0, 'Beautiful daisy!'),
       ('21ce1f3a-5313-4fd5-a487-9df482601dc4', 'chrysanthemum', false, null, 5.0, 'Oh, this Chrysanthemum!'),
       ('a1f729aa-0d4a-4c51-8c54-6a8df88944f1', 'tulip-red', false, null, 8.0, 'Red tulip'),
       ('b2f1134f-22aa-4b5b-9f53-2f41de997e21', 'tulip-yellow', false, null, 8.0, 'Yellow tulip'),
       ('c3a92ad1-90b8-44c5-9c14-8c751823783a', 'orchid', false, null, 25.0, 'Elegant orchid'),
       ('d4e44b43-5c28-442c-9b75-0f7a231f248c', 'lily', true, '2024-11-01T10:32:15Z', 12.0, 'White lily'),
       ('e5c72b31-0f5a-4c1c-b499-90dee99e1a10', 'sunflower', false, null, 7.0, 'Bright sunflower'),
       ('f6b6d6ec-33e6-4b86-a9fc-6a93dfaf2d89', 'violet', true, '2024-09-15T08:12:44Z', 4.0, 'Small violet'),
       ('0a1823f4-18b1-4b76-bf60-4e5fa8f5ec73', 'lavender', false, null, 9.0, 'Aromatic lavender'),
       ('1b2944f2-bb1f-4c77-b20b-67df931ac983', 'peony', false, null, 14.0, 'Pink peony'),
       ('2c30bd7c-63ad-4462-ba9b-60dfac78d44f', 'carnation', false, null, 6.0, 'Red carnation'),
       ('3d40c7e1-8945-48a4-9d87-5b99231b28cf', 'hydrangea', false, null, 18.0, 'Blue hydrangea'),
       ('4e509aa7-1428-4a51-9b92-0df546ae2c7d', 'lotus', true, '2024-10-20T14:05:03Z', 30.0, 'Sacred lotus'),
       ('5f60ab11-3c88-4b22-a92e-a55edee5e97b', 'magnolia', false, null, 20.0, 'Magnolia blossom');

INSERT INTO flowername(flowerid, languagecode, name)
VALUES ('7c77c1b8-0889-4685-9d84-6eb4500f8926', 'UA', 'Роза'),
       ('8bb48737-eb02-403e-8503-d8405d3bbeb4', 'UA', 'Ромашка'),
       ('21ce1f3a-5313-4fd5-a487-9df482601dc4', 'UA', 'Хризантема'),
       ('a1f729aa-0d4a-4c51-8c54-6a8df88944f1', 'UA', 'Червоний тюльпан'),
       ('b2f1134f-22aa-4b5b-9f53-2f41de997e21', 'UA', 'Жовтий тюльпан'),
       ('c3a92ad1-90b8-44c5-9c14-8c751823783a', 'UA', 'Орхідея'),
       ('d4e44b43-5c28-442c-9b75-0f7a231f248c', 'UA', 'Лілія'),
       ('e5c72b31-0f5a-4c1c-b499-90dee99e1a10', 'UA', 'Соняшник'),
       ('f6b6d6ec-33e6-4b86-a9fc-6a93dfaf2d89', 'UA', 'Фіалка'),
       ('0a1823f4-18b1-4b76-bf60-4e5fa8f5ec73', 'UA', 'Лаванда'),
       ('1b2944f2-bb1f-4c77-b20b-67df931ac983', 'UA', 'Півонія'),
       ('2c30bd7c-63ad-4462-ba9b-60dfac78d44f', 'UA', 'Гвоздика'),
       ('3d40c7e1-8945-48a4-9d87-5b99231b28cf', 'UA', 'Гортензія'),
       ('4e509aa7-1428-4a51-9b92-0df546ae2c7d', 'UA', 'Лотос'),
       ('5f60ab11-3c88-4b22-a92e-a55edee5e97b', 'UA', 'Магнолія'),

       ('7c77c1b8-0889-4685-9d84-6eb4500f8926', 'RO', 'Trandafir'),
       ('8bb48737-eb02-403e-8503-d8405d3bbeb4', 'RO', 'Margaretă'),
       ('21ce1f3a-5313-4fd5-a487-9df482601dc4', 'RO', 'Crizantemă'),
       ('a1f729aa-0d4a-4c51-8c54-6a8df88944f1', 'RO', 'Lalea roșie'),
       ('b2f1134f-22aa-4b5b-9f53-2f41de997e21', 'RO', 'Lalea galbenă'),
       ('c3a92ad1-90b8-44c5-9c14-8c751823783a', 'RO', 'Orhidee'),
       ('d4e44b43-5c28-442c-9b75-0f7a231f248c', 'RO', 'Crin'),
       ('e5c72b31-0f5a-4c1c-b499-90dee99e1a10', 'RO', 'Floarea soarelui'),
       ('f6b6d6ec-33e6-4b86-a9fc-6a93dfaf2d89', 'RO', 'Viorea'),
       ('0a1823f4-18b1-4b76-bf60-4e5fa8f5ec73', 'RO', 'Lavandă'),
       ('1b2944f2-bb1f-4c77-b20b-67df931ac983', 'RO', 'Bujor'),
       ('2c30bd7c-63ad-4462-ba9b-60dfac78d44f', 'RO', 'Garoafă'),
       ('3d40c7e1-8945-48a4-9d87-5b99231b28cf', 'RO', 'Hortensie'),
       ('4e509aa7-1428-4a51-9b92-0df546ae2c7d', 'RO', 'Lotus'),
       ('5f60ab11-3c88-4b22-a92e-a55edee5e97b', 'RO', 'Magnolie');

INSERT INTO categories(id, slug)
VALUES ('a1a1a1a1-1111-1111-1111-111111111111', 'wedding-flowers'),
       ('b2b2b2b2-2222-2222-2222-222222222222', 'garden-flowers'),
       ('c3c3c3c3-3333-3333-3333-333333333333', 'exotic-flowers'),
       ('d4d4d4d4-4444-4444-4444-444444444444', 'spring-flowers'),
       ('e5e5e5e5-5555-5555-5555-555555555555', 'summer-flowers'),
       ('f6f6f6f6-6666-6666-6666-666666666666', 'romantic-flowers');

INSERT INTO categoryname(categoryid, languagecode, name)
VALUES
('a1a1a1a1-1111-1111-1111-111111111111', 'UA', 'Весільні квіти'),
('b2b2b2b2-2222-2222-2222-222222222222', 'UA', 'Садові квіти'),
('c3c3c3c3-3333-3333-3333-333333333333', 'UA', 'Екзотичні квіти'),
('d4d4d4d4-4444-4444-4444-444444444444', 'UA', 'Весняні квіти'),
('e5e5e5e5-5555-5555-5555-555555555555', 'UA', 'Літні квіти'),
('f6f6f6f6-6666-6666-6666-666666666666', 'UA', 'Романтичні квіти'),

('a1a1a1a1-1111-1111-1111-111111111111', 'RO', 'Flori de nuntă'),
('b2b2b2b2-2222-2222-2222-222222222222', 'RO', 'Flori de grădină'),
('c3c3c3c3-3333-3333-3333-333333333333', 'RO', 'Flori exotice'),
('d4d4d4d4-4444-4444-4444-444444444444', 'RO', 'Flori de primăvară'),
('e5e5e5e5-5555-5555-5555-555555555555', 'RO', 'Flori de vară'),
('f6f6f6f6-6666-6666-6666-666666666666', 'RO', 'Flori romantice');

INSERT INTO flowercategory(flowerid, categoryid)
VALUES
-- Rose: Wedding, Romantic, Garden
('7c77c1b8-0889-4685-9d84-6eb4500f8926', 'a1a1a1a1-1111-1111-1111-111111111111'),
('7c77c1b8-0889-4685-9d84-6eb4500f8926', 'f6f6f6f6-6666-6666-6666-666666666666'),
('7c77c1b8-0889-4685-9d84-6eb4500f8926', 'b2b2b2b2-2222-2222-2222-222222222222'),

-- Daisy: Garden, Spring, Summer
('8bb48737-eb02-403e-8503-d8405d3bbeb4', 'b2b2b2b2-2222-2222-2222-222222222222'),
('8bb48737-eb02-403e-8503-d8405d3bbeb4', 'd4d4d4d4-4444-4444-4444-444444444444'),
('8bb48737-eb02-403e-8503-d8405d3bbeb4', 'e5e5e5e5-5555-5555-5555-555555555555'),

-- Chrysanthemum: Garden, Summer
('21ce1f3a-5313-4fd5-a487-9df482601dc4', 'b2b2b2b2-2222-2222-2222-222222222222'),
('21ce1f3a-5313-4fd5-a487-9df482601dc4', 'e5e5e5e5-5555-5555-5555-555555555555'),

-- Red Tulip: Wedding, Spring, Romantic
('a1f729aa-0d4a-4c51-8c54-6a8df88944f1', 'a1a1a1a1-1111-1111-1111-111111111111'),
('a1f729aa-0d4a-4c51-8c54-6a8df88944f1', 'd4d4d4d4-4444-4444-4444-444444444444'),
('a1f729aa-0d4a-4c51-8c54-6a8df88944f1', 'f6f6f6f6-6666-6666-6666-666666666666'),

-- Yellow Tulip: Spring, Garden
('b2f1134f-22aa-4b5b-9f53-2f41de997e21', 'd4d4d4d4-4444-4444-4444-444444444444'),
('b2f1134f-22aa-4b5b-9f53-2f41de997e21', 'b2b2b2b2-2222-2222-2222-222222222222'),

-- Orchid: Exotic, Wedding, Romantic
('c3a92ad1-90b8-44c5-9c14-8c751823783a', 'c3c3c3c3-3333-3333-3333-333333333333'),
('c3a92ad1-90b8-44c5-9c14-8c751823783a', 'a1a1a1a1-1111-1111-1111-111111111111'),
('c3a92ad1-90b8-44c5-9c14-8c751823783a', 'f6f6f6f6-6666-6666-6666-666666666666'),

-- Sunflower: Summer, Garden
('e5c72b31-0f5a-4c1c-b499-90dee99e1a10', 'e5e5e5e5-5555-5555-5555-555555555555'),
('e5c72b31-0f5a-4c1c-b499-90dee99e1a10', 'b2b2b2b2-2222-2222-2222-222222222222'),

-- Lavender: Garden, Summer, Romantic
('0a1823f4-18b1-4b76-bf60-4e5fa8f5ec73', 'b2b2b2b2-2222-2222-2222-222222222222'),
('0a1823f4-18b1-4b76-bf60-4e5fa8f5ec73', 'e5e5e5e5-5555-5555-5555-555555555555'),
('0a1823f4-18b1-4b76-bf60-4e5fa8f5ec73', 'f6f6f6f6-6666-6666-6666-666666666666'),

-- Peony: Wedding, Romantic, Spring
('1b2944f2-bb1f-4c77-b20b-67df931ac983', 'a1a1a1a1-1111-1111-1111-111111111111'),
('1b2944f2-bb1f-4c77-b20b-67df931ac983', 'f6f6f6f6-6666-6666-6666-666666666666'),
('1b2944f2-bb1f-4c77-b20b-67df931ac983', 'd4d4d4d4-4444-4444-4444-444444444444'),

-- Carnation: Garden, Romantic
('2c30bd7c-63ad-4462-ba9b-60dfac78d44f', 'b2b2b2b2-2222-2222-2222-222222222222'),
('2c30bd7c-63ad-4462-ba9b-60dfac78d44f', 'f6f6f6f6-6666-6666-6666-666666666666'),

-- Hydrangea: Garden, Summer, Wedding
('3d40c7e1-8945-48a4-9d87-5b99231b28cf', 'b2b2b2b2-2222-2222-2222-222222222222'),
('3d40c7e1-8945-48a4-9d87-5b99231b28cf', 'e5e5e5e5-5555-5555-5555-555555555555'),
('3d40c7e1-8945-48a4-9d87-5b99231b28cf', 'a1a1a1a1-1111-1111-1111-111111111111'),

-- Magnolia: Exotic, Spring
('5f60ab11-3c88-4b22-a92e-a55edee5e97b', 'c3c3c3c3-3333-3333-3333-333333333333'),
('5f60ab11-3c88-4b22-a92e-a55edee5e97b', 'd4d4d4d4-4444-4444-4444-444444444444');