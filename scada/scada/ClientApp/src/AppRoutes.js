import { Counter } from "./components/Counter";
import { Trending } from "./components/Trending/Trending";
import { Login } from "./components/Login/Login";
import Reports from "./components/Reports/Reports";

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
  },
    {
        path: '/reports',
        element: <Reports />
    }
];

export default AppRoutes;
