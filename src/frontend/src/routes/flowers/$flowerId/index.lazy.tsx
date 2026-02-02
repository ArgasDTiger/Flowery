import { createLazyFileRoute } from '@tanstack/react-router';

export const Route = createLazyFileRoute('/flowers/$flowerId/')({
  component: FlowerDetailComponent,
});

function FlowerDetailComponent() {

  return (<></>);
}
