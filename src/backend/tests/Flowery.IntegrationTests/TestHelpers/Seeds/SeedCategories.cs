namespace Flowery.IntegrationTests.TestHelpers.Seeds;

public static class SeedCategories
{
    public const string SeedCategoriesSql = @"
        INSERT INTO categories(id, slug)
        VALUES
            ('a1a1a1a1-1111-1111-1111-111111111111', 'wedding-flowers'),
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
";
}