import { createLazyFileRoute } from "@tanstack/react-router";

export const Route = createLazyFileRoute('/home/')({
  component: Home,
});

function Home()  {
  return (
    <div>
      <h1>Home!</h1>
    </div>
  );
}

export default Home;