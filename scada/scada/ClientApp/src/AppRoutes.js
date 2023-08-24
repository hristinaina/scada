import { Counter } from "./components/Counter";
import { Trending } from "./components/Trending/Trending";
import { Login } from "./components/Login/Login";
import { DatabaseManager } from "./components/DatabaseManager/DatabaseManager";

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
      path: '/database-manager',
      element: <DatabaseManager/>
  }
];

export default AppRoutes;
