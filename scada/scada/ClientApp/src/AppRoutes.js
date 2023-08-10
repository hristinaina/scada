import { Counter } from "./components/Counter";
import { Trending } from "./components/Trending/Trending";
import { Login } from "./components/Login/Login";

const AppRoutes = [
  {
    path: '/',
    index: true,
    element: <Login />
  },
  {
    path: '/counter',
      element: <Counter />
  },
  {
      path: '/trending',
      element: <Trending />
  }
];

export default AppRoutes;
