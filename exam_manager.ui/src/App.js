import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navbar from "./components/NavbarComponent"; // Import the Navbar
import LimitedRoutes from "./utilities/LimitedRoutes";
import Login from "./pages/Login/LoginPage";
import Dashboard from "./pages/Home/DashboardPage";
import CheckUserLogin from "./utilities/CheckUserLogin";
import Exams from "./pages/Exams/ExamsPage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Navbar is parent because of maintainability */}
        <Route path="/" element={<Navbar />}>
          {/* If user is not logged in, restrict these routes */}
          <Route element={<CheckUserLogin />}>
            <Route path="/" element={<Dashboard />} />
            <Route path="/exams" element={<Exams />} />
          </Route>

          {/* If user is logged in, restrict these routes */}
          <Route element={<LimitedRoutes />}>
            <Route path="/login" element={<Login />} />
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
