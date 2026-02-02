import { createLazyFileRoute } from '@tanstack/react-router'

export const Route = createLazyFileRoute('/flowers/create/')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/flowers/create/"!</div>
}
