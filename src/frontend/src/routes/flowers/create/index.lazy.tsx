import { createLazyFileRoute, useNavigate } from '@tanstack/react-router';
import { CreateFlowerComponent, useCreateFlower } from "@features/flowers/create-flower";
import { toast } from "sonner";

export const Route = createLazyFileRoute('/flowers/create/')({
  component: RouteComponent,
});

function RouteComponent() {
  const navigate = useNavigate();

  const createFlowerMutation = useCreateFlower({
    mutationConfig: {
      onSuccess: (slug) => {
        toast.success("Flower created successfully!");
        navigate({ to: '/flowers/$flowerId', params: { flowerId: slug } });
      },
      onError: () => {
        toast.error("Failed to create flower. Please try again.");
      },
    },
  });

  return (
    <CreateFlowerComponent
      onSubmit={(data) => createFlowerMutation.mutate(data)}
      isPending={createFlowerMutation.isPending}
    />
  );
}

