import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";

import LimitedRoutes from "./utilities/LimitedRoutes";

import Navigation from "./components/NavigationComponent";

import Login from "./pages/LoginPage";
import Dashboard from "./pages/DashboardPage";
import CheckUserLogin from "./utilities/CheckUserLogin";

function App() {

  return (
    <BrowserRouter>
      <Routes>
        <Route element={<CheckUserLogin />}>
          <Route path="/" element={<Navigation />}>
            <Route path="/" element={<Dashboard />} />
          </Route>
        </Route>

        {/* Routes without navbar */}
        <Route element={<LimitedRoutes />}>
          <Route path="/login" element={<Login />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
export default App;
