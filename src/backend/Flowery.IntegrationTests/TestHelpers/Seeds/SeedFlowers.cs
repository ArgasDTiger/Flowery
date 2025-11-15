
namespace Flowery.IntegrationTests.TestHelpers.Seeds;

public static class SeedFlowers
{
    public const string SeedFlowersSql = @"
        INSERT INTO flowers(id, slug, isdeleted, deletedatutc, price, description)
        VALUES
            ('7c77c1b8-0889-4685-9d84-6eb4500f8926', 'rose', false, null, 10.0, 'A white rose'),
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
        VALUES 
               ('7c77c1b8-0889-4685-9d84-6eb4500f8926', 'UA', 'Роза'),
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
               ('5f60ab11-3c88-4b22-a92e-a55edee5e97b', 'UA', 'Магнолія');
";

}