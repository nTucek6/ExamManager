import "./App.css";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";

import LimitedRoutes from "./utilities/LimitedRoutes";

import Navigation from "./components/NavigationComponent";

import Login from "./pages/LoginPage";
import Dashboard from "./pages/DashboardPage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<LimitedRoutes />}>
          <Route path="/" element={<Navigate to="/login" replace />} />
          <Route path="/login" element={<Login />} />
        </Route>

        <Route element={<><Navigation /></>}>
          <Route path="/dashboard" element={<Dashboard />} />
        </Route>
        {/* Routes without navbar */}
      </Routes>
    </BrowserRouter>
  );
}

export default App;
