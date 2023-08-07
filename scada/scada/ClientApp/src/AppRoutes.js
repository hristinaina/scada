import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Login } from "./components/Login";

const AppRoutes = [
  {
    index: true,
    element: <Login />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  }
];

export default AppRoutes;
