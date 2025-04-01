import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navbar from "./components/NavbarComponent"; // Import the Navbar
import LimitedRoutes from "./utilities/LimitedRoutes";
import Login from "./pages/Login/LoginPage";
import Register from "./pages/Register/RegisterPage.js";
import Dashboard from "./pages/Home/DashboardPage";
import CheckUserLogin from "./utilities/CheckUserLogin";
import Exams from "./pages/Exams/ExamsPage";
import CheckProfessorRole from "./utilities/CheckProfessorRole";
import AddExam from "./pages/AddExam/AddExamPage.js";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* If user is not logged in, restrict these routes */}
        <Route element={<CheckUserLogin />}>
          {/* Navbar is parent because of maintainability */}
          <Route path="/" element={<Navbar />}>
            <Route path="/" element={<Dashboard />} />
            <Route path="/exams" element={<Exams />} />

            {/* Professor routes */}
            <Route element={<CheckProfessorRole />}>
              <Route path="/addexam" element={<AddExam />} />
            </Route>
          </Route>
        </Route>
        {/* If user is logged in, restrict these routes */}
        <Route element={<LimitedRoutes />}>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
