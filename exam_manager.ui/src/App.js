import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navbar from "./components/NavbarComponent"; // Import the Navbar
import LimitedRoutes from "./utilities/LimitedRoutes";
import Login from "./pages/LoginPage";
import Dashboard from "./pages/DashboardPage";
import CheckUserLogin from "./utilities/CheckUserLogin";

function App() {
  return (
    <BrowserRouter>
      <Navbar /> {/* Navbar placed outside Routes */}
      <Routes>
        <Route element={<CheckUserLogin />}>
          <Route path="/" element={<Dashboard />} />
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
