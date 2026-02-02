import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/flowers/create/')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/flowers/create/"!</div>
}
