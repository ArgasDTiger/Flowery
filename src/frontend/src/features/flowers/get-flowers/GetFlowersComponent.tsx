import { Badge, Button, Card, ComboboxData, Group, Pagination, Select, Skeleton, Text, TextInput } from "@mantine/core";
import { GetFlowersResponse, GetFlowersSortField } from "@features/flowers/get-flowers/types";
import { Search, ShoppingCart } from "lucide-react";
import { Link } from "@tanstack/react-router";
import styles from './GetFlowersComponent.module.scss';
import { toast } from "sonner";
import React from "react";
import { SortDirection } from "@features/shared/pagination";

interface Props {
  flowers: GetFlowersResponse[];
  totalCount: number;
  activePage: number;
  pageSize: number;
  onPageChange: (page: number) => void;
  searchValue: string;
  onSearchChange: (value: string) => void;
  onSearchCommit: () => void;
  onCategoryChange: (category: string | null) => void;
  onSortByChange: (sortBy: GetFlowersSortField | null) => void;
  onSortDirectionChange: (sortDirection: SortDirection | null) => void;
  selectedCategory: string | null;
  sortBy: GetFlowersSortField | null;
  sortDirection: SortDirection | null;
  isPending: boolean;
  categories: ComboboxData;
}

const sortByOptions = [
  { value: GetFlowersSortField.Price, label: 'Price' },
  { value: GetFlowersSortField.Date, label: 'Date' },
];

const sortDirectionOptions = [
  { value: SortDirection.Asc, label: 'Ascending' },
  { value: SortDirection.Desc, label: 'Descending' },
];

export const GetFlowersComponent = ({
                                      flowers,
                                      totalCount,
                                      activePage,
                                      pageSize,
                                      onPageChange,
                                      searchValue,
                                      onSearchChange,
                                      onSearchCommit,
                                      onCategoryChange,
                                      onSortByChange,
                                      onSortDirectionChange,
                                      selectedCategory,
                                      sortBy,
                                      sortDirection,
                                      isPending,
                                      categories,
                                    }: Props) => {
  const totalPages = Math.ceil(totalCount / pageSize);

  const handleAddToBasket = (e: React.MouseEvent<HTMLButtonElement>, flowerId: string) => {
    e.preventDefault();
    e.stopPropagation();
    console.log(flowerId);
    toast.success(`Flower added to basket!`);
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter') {
      onSearchCommit();
    }
  };

  return (
    <section>
      <section className={styles.header}>
        <h1>Flowers Catalog</h1>
        <Link to="/flowers/create">
          <Button className="black">Create New Flower</Button>
        </Link>
      </section>

      <section className={styles.filtersSection}>
        <div className={styles.filters}>
          <div className={styles.searchWrapper}>
            <TextInput
              placeholder="Search..."
              value={searchValue}
              onChange={(e) => onSearchChange(e.currentTarget.value)}
              onKeyDown={handleKeyDown}
              className={styles.searchInput}
            />
            <Button className="black" onClick={onSearchCommit}>
              <Search size={18} />
            </Button>
          </div>

          <Select
            placeholder="All categories"
            data={categories}
            value={selectedCategory}
            onChange={onCategoryChange}
            clearable
            className={styles.categoryFilter}
          />

          <Select
            placeholder="Sort by"
            data={sortByOptions}
            value={sortBy}
            onChange={(value) => onSortByChange(value as GetFlowersSortField | null)}
            clearable
            className={styles.sortSelect}
          />

          <Select
            placeholder="Direction"
            data={sortDirectionOptions}
            value={sortDirection}
            onChange={(value) => onSortDirectionChange(value as SortDirection | null)}
            clearable
            disabled={!sortBy}
            className={styles.sortSelect}
          />
        </div>
      </section>

      <section className={styles.flowersSection}>
        <ul className={styles.flowersGrid}>
          {isPending && (
            Array.from({ length: pageSize }).map((_, index) => (
              <li key={index}>
                <Card shadow="sm" padding="lg" radius="md" withBorder className={styles.flowerCard}>
                  <Card.Section>
                    <Skeleton height={300} />
                  </Card.Section>
                  <Skeleton height={20} mt="md" width="70%" />
                  <Skeleton height={15} mt="xs" width="40%" />
                  <Skeleton height={30} mt="md" />
                </Card>
              </li>
            ))
          )}
          {!isPending && (flowers.length === 0 ? (
            <li className={styles.noResults}>
              <Text size="lg">No flowers match your search result</Text>
            </li>
          ) : (
            flowers.map((flower) => (
              <li key={flower.id}>
                <article className={styles.flowerCard}>
                  <Link to="/flowers/$flowerId" params={{ flowerId: flower.slug }} className={styles.flowerLink}>
                    <Card shadow="sm" padding="lg" radius="md" withBorder>
                      <Card.Section>
                        <figure className={styles.imagePlaceholder}>
                          <Skeleton height={300}>
                            <div className={styles.noImage}>No image is available</div>
                          </Skeleton>
                        </figure>
                      </Card.Section>

                      <Group justify="space-between" mt="md" mb="xs">
                        <h2>{flower.name}</h2>
                      </Group>

                      {flower.categories && flower.categories.length > 0 && (
                        <Group gap="xs" mb="md" component="div">
                          {flower.categories.map((category, index) => (
                            <Badge key={index} color="blue" variant="light">
                              {category}
                            </Badge>
                          ))}
                        </Group>
                      )}

                      <Group justify="space-between" mt="md">
                        <Text size="xl" fw={700} c="blue" component="span">
                          ₴{flower.price.toFixed(2)}
                        </Text>
                        <Button
                          className="black"
                          leftSection={<ShoppingCart size={16} />}
                          onClick={(e) => handleAddToBasket(e, flower.id)}
                        >
                          Add to Basket
                        </Button>
                      </Group>
                    </Card>
                  </Link>
                </article>
              </li>
            ))
          ))}
        </ul>
      </section>

      {totalPages > 1 && (
        <nav>
          <Pagination
            total={totalPages}
            value={activePage}
            onChange={onPageChange}
            mt="xl"
            className={styles.pagination}
          />
        </nav>
      )}
    </section>
  );
};
