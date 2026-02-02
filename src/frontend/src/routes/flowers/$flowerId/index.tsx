import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/flowers/$flowerId/')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/flowers/$flowerId/"!</div>
}
